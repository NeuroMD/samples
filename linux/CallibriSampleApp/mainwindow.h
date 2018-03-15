#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include "scanner_factory.h"
#include <atomic>
#include "chart.h"
#include <QtCharts/QChartView>
#include "subsystem/signal_subsystem.h"
#include "method/ecg_device.h"
#include "method/bfb_device.h"
#include <atomic>

namespace Ui {
class MainWindow;
}
struct UIData
{
    QListPointF_t ecgData;
    RPeakPoint_t rPeaks;
    int batteryLevel;
    int heartRate;
    double heartRateVariability;
    int bfbIndex;
};

Q_DECLARE_METATYPE(std::shared_ptr<NeuroDevice>) // !!!!
Q_DECLARE_METATYPE(DeviceState) // !!!!
Q_DECLARE_METATYPE(UIData) // !!!!


class MainWindow : public QMainWindow
{
    Q_OBJECT

signals:
     void updateScanStateSignal(const bool &state);
     void addItemSignal(const std::shared_ptr<NeuroDevice> & devItm);
     void devCurrStateChangedSignal(const DeviceState & state);
     void renderDeviceDataSignal(const UIData&);

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();

private slots:
    void on_btnSearch_clicked();
    void updateScanState(const bool &state);
    void addItem(const std::shared_ptr<NeuroDevice> & devItm);
    void devCurrStateChanged(const DeviceState & state);
    void renderDeviceData(const UIData&);

    void on_btnConnect_clicked();

    void on_btnStart_clicked();

    void on_calibrationButton_clicked();

    void on_btnPause_clicked();

private:
    Ui::MainWindow *ui;
    decltype(getNeuroConnectionScanner()) scanner;
    std::atomic<bool> scanning;
    std::vector<std::shared_ptr<NeuroDevice>> devices;
    std::shared_ptr<NeuroDevice> deviceCurrent;
    DeviceState deviceCurrentState;
    Chart *chart;
    QChartView* chartView;

    EventHandler(NeuroDevice, deviceStateChanged) deviceStateChangedHandler;

    std::atomic<bool> renderRunning;
    std::atomic<bool> renderPause;
    double durationCurrent;
    std::atomic<bool> needCalibration;

    template <class Device>
    void renderThread(Device *);

    template <class Device>
    void renderStart(std::unique_ptr<Device>);

    void renderStop();
    bool isRenderRuning();
};

#endif // MAINWINDOW_H
