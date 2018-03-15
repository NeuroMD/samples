#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QDesktopWidget>
#include <string>

#include <iostream>
#include <thread>
#include "method/ecg_device.h"


MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow),
    scanner(getNeuroConnectionScanner())
{
    renderRunning = false;
    qRegisterMetaType<std::shared_ptr<NeuroDevice>>("NeuroDevice"); // !!!!
    qRegisterMetaType<DeviceState>("DeviceState"); // !!!!
    qRegisterMetaType<UIData>("HRVData"); // !!!!


    // connect to own signal to own slot and hence "translate" it
    connect(this, SIGNAL(updateScanStateSignal(const bool&)),
            this, SLOT(updateScanState(const bool&)));
    connect(this, SIGNAL(addItemSignal(const std::shared_ptr<NeuroDevice> &)),
            this, SLOT(addItem(const std::shared_ptr<NeuroDevice> &)));
    connect(this, SIGNAL(devCurrStateChangedSignal(const DeviceState &)),
            this, SLOT(devCurrStateChanged(const DeviceState &)));
    connect(this, SIGNAL(renderDeviceDataSignal(const UIData&)),
            this, SLOT(renderDeviceData(const UIData&)));

    ui->setupUi(this);
    // Center screen
    this->setGeometry(
        QStyle::alignedRect(
            Qt::LeftToRight,
            Qt::AlignCenter,
            this->size(),
            qApp->desktop()->availableGeometry()
        )
    );
    ui->progressBar->setVisible(false);

    QStringList strHeaderlist;
    strHeaderlist << "Name" << "Address";
    ui->twDeviceList->setColumnCount(2);
    ui->twDeviceList->setHorizontalHeaderLabels(strHeaderlist);
    ui->twDeviceList->setSelectionBehavior(QAbstractItemView::SelectRows);
    QHeaderView* header = ui->twDeviceList->horizontalHeader();
    header->setSectionResizeMode(QHeaderView::Stretch);

    scanner->subscribeDeviceFound([this](std::shared_ptr<NeuroDevice> dev) {
        emit addItemSignal(dev);
    });
    scanner->subscribeScanStateChanged([this](bool state) {
        emit updateScanStateSignal(state);
    });
    scanning = false;
    ui->progressBar->setMaximum(0);
    ui->progressBar->setMinimum(0);

    ui->btnConnect->setEnabled(false);
    ui->btnStart->setEnabled(false);
    ui->btnPause->setEnabled(false);
    deviceCurrentState = DeviceState::UNKNOWN;

    // chart
    chart = new Chart;
    chart->setTitle("Signal");
    chart->legend()->hide();
    chart->setAnimationOptions(QChart::NoAnimation);
    chart->setMinimumHeight(300);
    chartView = new QChartView(chart);
    chartView->setRenderHint(QPainter::Antialiasing);
    ui->signalLayout->addWidget(chartView);
    chartView->setVisible(false);
    ui->hrvDataWidget->setVisible(false);
}

void MainWindow::updateScanState(const bool &state)
{
    scanning = state;
    ui->btnSearch->setText(state ? QString("Stop") : QString("Scan"));
    if(state)
    {
        ui->progressBar->setValue(0);
        ui->progressBar->setVisible(true);
    } else
    {
        ui->progressBar->setVisible(false);
        ui->btnConnect->setEnabled(true);
    }
}

void MainWindow::addItem(const std::shared_ptr<NeuroDevice> & devItm)
{
    devices.push_back(devItm);
    auto twDeviceList = ui->twDeviceList;
    std::cout << devItm->getName() << std::endl;
    twDeviceList->setRowCount(twDeviceList->model()->rowCount() + 1);
    QTableWidgetItem* item = new QTableWidgetItem();
    item->setFlags(Qt::ItemIsSelectable | Qt::ItemIsEnabled);
    item->setText(QString(devItm->getName().c_str()));
    twDeviceList->setItem(twDeviceList->model()->rowCount() - 1, 0, item);

    item = new QTableWidgetItem();
    item->setFlags(Qt::ItemIsSelectable | Qt::ItemIsEnabled);
    item->setText(QString(devItm->getAddress().c_str()));
    twDeviceList->setItem(twDeviceList->model()->rowCount() - 1, 1, item);
}

MainWindow::~MainWindow()
{
    renderStop();
    scanner.reset();
    delete ui;
}

void MainWindow::on_btnSearch_clicked()
{
    if(scanning)
    {
        try {
            scanner->stopScan();
        } catch (std::exception &ex) {
            std::cout << ex.what() << std::endl;
            ui->statusBar->showMessage(QString(ex.what()));
        }
        return;
    }
    ui->twDeviceList->clearContents();
    ui->twDeviceList->setRowCount(0);
    devices.clear();

    ui->statusBar->clearMessage();
    try {
//        test();
        scanner->startScan(5000);
//        scanner->findDeviceByAddress(std::string("EE:6D:7B:BE:44:36"));

        ui->btnConnect->setEnabled(false);
    } catch (std::exception &ex) {
        std::cout << ex.what() << std::endl;
        ui->statusBar->showMessage(QString(ex.what()));
    }
}

std::shared_ptr<NeuroDevice> findeByAddressInV(const std::vector<std::shared_ptr<NeuroDevice>> &devices, const std::string &address)
{
    if(!devices.empty()) {
        auto pred = [&](const std::shared_ptr<NeuroDevice> & item) {
            return item->getAddress().compare(address) == 0;
        };
        auto devRes = std::find_if(std::begin(devices), std::end(devices), pred);
        if(devRes != std::end(devices))
            return *devRes;
    }
    return nullptr;
}

std::string stateToString(const DeviceState & state)
{
//    CLOSED=6
    switch (state) {
    case DeviceState::DISCONNECTED:
        return std::string("DISCONNECTED");
    case DeviceState::CONNECTED:
        return std::string("CONNECTED");
    case DeviceState::WORKING:
        return std::string("WORKING");
    default:
        return std::string("UNKNOWN");
    }
}
bool devIsSupportSignalSystem(const std::vector<DeviceFeature> & features)
{
    for (const auto& featureItem : features)
        if(featureItem == DeviceFeature::SIGNAL)
            return true;
    return false;
}
void MainWindow::devCurrStateChanged(const DeviceState & state)
{
    ui->btnStart->setEnabled(false);
    if((state == DeviceState::CONNECTED || state == DeviceState::WORKING)  && devIsSupportSignalSystem(deviceCurrent->getFeatures()))
    {
        ui->btnConnect->setText(QString("Disconnect"));
        ui->btnConnect->setEnabled(true);
        ui->btnStart->setEnabled(true);
    }
    else if(state == DeviceState::DISCONNECTED
            || state == DeviceState::UNKNOWN)
    {
        ui->btnConnect->setText(QString("Connect"));
        ui->btnConnect->setEnabled(true);
    }

    ui->btnPause->setEnabled(false);
    ui->btnPause->setText("Pause");

    ui->statusBar->showMessage(QString().sprintf("State: %s", stateToString(state).c_str()));
    deviceCurrentState = state;
}

void MainWindow::on_btnConnect_clicked()
{
    if(deviceCurrent != nullptr)
    {
        deviceCurrent->disconnect();
        deviceCurrent = nullptr;
        return;
    }

    auto twDeviceList = ui->twDeviceList;
    if(!twDeviceList->selectionModel()->hasSelection())
        return;
    std::string addrSel = twDeviceList->selectionModel()->model()->index(twDeviceList->selectionModel()->currentIndex().row(), 1).data().toString().toStdString();
    std::shared_ptr<NeuroDevice> devSel = findeByAddressInV(devices, addrSel);
    if(devSel == nullptr)
        return;
    ui->btnConnect->setEnabled(false);

    deviceCurrent = devSel;

    auto deviceStateHandler = [this, &devSel](const NeuroDevice &, DeviceState state, DeviceError) {emit devCurrStateChangedSignal(state);};
    deviceStateChangedHandler = MakeHandler(NeuroDevice, deviceStateChanged, deviceStateHandler);
    devSel->deviceStateChanged += deviceStateChangedHandler;
    devSel->connect();
}

void MainWindow::on_btnStart_clicked()
{
    if(deviceCurrent == nullptr)
        return;
    if(deviceCurrentState == DeviceState::CONNECTED
            || deviceCurrentState == DeviceState::WORKING)
    {
        if(isRenderRuning())
        {
            ui->btnStart->setText(QString("Start"));
            ui->btnPause->setEnabled(false);
            ui->btnPause->setText("Pause");
            renderStop();
        } else
        {
            chartView->setVisible(true);
            ui->hrvDataWidget->setVisible(true);
//            renderStart(std::unique_ptr<BfbDevice>(new BfbDevice(deviceCurrent)));
            renderStart(std::unique_ptr<EcgDevice>(new EcgDevice(deviceCurrent)));
            ui->btnStart->setText(QString("Stop"));
            renderPause.store(false);
            ui->btnPause->setEnabled(true);
            ui->btnPause->setText("Pause");
        }
    }
}

long getTimeMs() {
    auto now = std::chrono::system_clock::now();
    return std::chrono::time_point_cast<std::chrono::milliseconds>(now).time_since_epoch().count();
}

template<>
void MainWindow::renderThread(EcgDevice *device_ptr) {
    std::unique_ptr<EcgDevice> device(device_ptr);
    auto nDevice = device->getNeuroDevice();
    device->startReceive();
    durationCurrent = device->getTotalDuration();
    double duration, durationRR;
    static const double durationRROffset = 3;
    int bLevel = nDevice->getBatteryLevel();
    static const long bLevelInterval = 5000;
    long lastUpdateBLevel =  getTimeMs() + bLevelInterval, currTime;
    static const long TIME_SLEEP = 30;


    while (renderRunning) {
        currTime = getTimeMs();
        if(lastUpdateBLevel < currTime) { // update data device power level information
            lastUpdateBLevel = currTime + bLevelInterval;
            bLevel = nDevice->getBatteryLevel();
        }
        std::this_thread::sleep_for(std::chrono::milliseconds(TIME_SLEEP));
        if(renderPause.load()) {
            if(!device->isPauseReceive())
                device->pauseReceive();
            continue;
        } else if(device->isPauseReceive()) {
            device->resumeReceive();
        }

        duration = device->getTotalDuration() - durationCurrent;
        if(duration <= 0)
            continue;

        std::vector<SignalSample> signal = device->readEcgSignal(durationCurrent, duration);
        if(signal.empty())
            continue;

        QListPointF_t signalPoints;
        qreal valX, mulX = duration/signal.size();
        qreal valY;

        for(double i=0; i<signal.size(); i+=1) {
            valX = durationCurrent + (mulX * i);
            valY = signal.at(size_t(i));
            signalPoints.push_back(QPointF(valX, valY));
        }

        durationRR = durationCurrent - durationRROffset;
        if(durationRR < 0)
            durationRR = 0;
        RPeakPoint_t rPeak = device->getRPeaks(durationRR, durationCurrent + duration);
        durationCurrent += duration;

        UIData hrvData = {signalPoints, rPeak, bLevel, device->getHeartRate(), device->getStressIndex(), 0};
        emit renderDeviceDataSignal(hrvData);
    }
    device->stopReceive();
    renderRunning = false;
}

template<>
void MainWindow::renderThread(BfbDevice *device_ptr) {
    std::unique_ptr<BfbDevice> device(device_ptr);
    auto nDevice = device->getNeuroDevice();
    //creating index with 5-15Hz band and 1 second window
    auto bfbIndex = device->createIndex(5,50,1.0, 50);
    needCalibration.store(false);
    device->startReceive();
    auto signalSubsystem = nDevice->getSignalSubsystem().lock();
    if (!signalSubsystem){
        std::cerr << "Signal subsystem destroyed" << std::endl;
        return;
    }
    auto firstChannel = signalSubsystem->getChannels()[0].lock();
    if (!firstChannel){
        std::cerr << "Channel destroyed" << std::endl;
        return;
    }

    durationCurrent = static_cast<double>(firstChannel->getDataLength())/signalSubsystem->getSamplingFrequency();
    double duration;
    static const long TIME_SLEEP = 30;


    while (renderRunning) {

        if (needCalibration.load()){
            bfbIndex->calibrate();
            needCalibration.store(false);
        }
        std::this_thread::sleep_for(std::chrono::milliseconds(TIME_SLEEP));

        duration = static_cast<double>(firstChannel->getDataLength())/signalSubsystem->getSamplingFrequency() - durationCurrent;
        if(duration <= 0)
            continue;

        std::vector<SignalSample> signal = firstChannel->getSignalData(static_cast<std::size_t>(durationCurrent*signalSubsystem->getSamplingFrequency()),
                                                                          static_cast<std::size_t>(duration*signalSubsystem->getSamplingFrequency()));
        if(signal.empty())
            continue;

        QListPointF_t signalPoints;
        qreal valX, mulX = duration/signal.size();
        qreal valY;

        for(double i=0; i<signal.size(); i+=1) {
            valX = durationCurrent + (mulX * i);
            valY = signal.at(size_t(i));
            signalPoints.push_back(QPointF(valX, valY));
        }

        durationCurrent += duration;

        UIData uiData = {signalPoints, RPeakPoint_t(), nDevice->getBatteryLevel(), 0, 0, bfbIndex->value()};
        emit renderDeviceDataSignal(uiData);
    }
    device->stopReceive();
    renderRunning = false;
}

void MainWindow::renderDeviceData(const UIData & uiData)
{
    chart->renderDeviceData(uiData.ecgData, uiData.rPeaks);
    ui->lcdHeartRate->display(uiData.heartRate);
    ui->lcdHRV->display(uiData.heartRateVariability);
    ui->lcdBattery->display(uiData.batteryLevel);
    ui->lcdBfbIndex->display(uiData.bfbIndex);
}

template <class Device>
void MainWindow::renderStart(std::unique_ptr<Device> nDevice)
{
    if(isRenderRuning())
        renderStop();
    if(nDevice == nullptr)
        return;


    renderRunning.store(true);
    auto nDevicePtr = nDevice.release();
    std::thread thDraw([=]{
        renderThread(nDevicePtr);
    });
    thDraw.detach();
}

void MainWindow::renderStop()
{
    renderRunning.store(false);
}

bool MainWindow::isRenderRuning()
{
    return renderRunning.load();
}

void MainWindow::on_calibrationButton_clicked()
{
    if (!needCalibration.load()){
        needCalibration.store(true);
    }
}

void MainWindow::on_btnPause_clicked()
{
    if(deviceCurrent == nullptr)
        return;
    if(!renderPause.load()) {
        renderPause.store(true);
        ui->btnPause->setText("Resume");
    } else {
        renderPause.store(false);
        ui->btnPause->setText("Pause");
    }
}
