
#include "chart.h"

#include <QtCharts/QValueAxis>
#include <QPoint>
#include <QPen>

class VLineSeries : public QXYSeries
{
public:

};
Chart::Chart(QGraphicsItem *parent, Qt::WindowFlags wFlags):
    QChart(QChart::ChartTypeCartesian, parent, wFlags),
    seriesECG(0),
    axis(new QValueAxis)
{
    seriesECG = new QSplineSeries(this);
    QPen penECG(Qt::red);
    penECG.setWidth(1);
    seriesECG->setPen(penECG);

    seriesRR = new QLineSeries(this);
    QPen penRR(Qt::blue);
    penRR.setWidthF(0.25f);
    penRR.setStyle(Qt::PenStyle::DotLine);
    penRR.setCapStyle(Qt::PenCapStyle::FlatCap);
    seriesRR->setPen(penRR);


    addSeries(seriesRR);
    addSeries(seriesECG);

    createDefaultAxes();
    setAxisX(axis, seriesECG);
    setAxisX(axis, seriesRR);

    axisX()->setRange(0, timeWindow);

    axisX()->setGridLineVisible(false);
    axisY()->setVisible(false);
    /*axisY()->setLabelsVisible(false)*/;
}

Chart::~Chart()
{
}

void Chart::renderDeviceData(const QListPointF_t &dataPoint, const RPeakPoint_t &rPeak)
{
    if(!dataPoint.empty())
    {
        for (const auto& dataPointItem : dataPoint)
            seriesECG->append(dataPointItem);

        if(!rPeak.empty())
        {
            double x;
            int idxRR = 0;
            for (const auto& rPeakItem : rPeak)
            {
                x = rPeakItem.time();
                while(idxRR < seriesRR->count() && seriesRR->at(idxRR).x() != x)
                    idxRR++;
                if(idxRR < seriesRR->count() && seriesRR->at(idxRR).x() == x)
                    continue;
                seriesRR->append(x, 0);
                seriesRR->append(x, 1);
                seriesRR->append(x, -1);
                seriesRR->append(x, 0);
            }
        }

        if((dataPoint.back().x() - seriesECG->at(0).x()) > timeWindow)
        {
            qreal firstX = dataPoint.at(0).x();
            qreal lastX = dataPoint.back().x();

            int cnt = 0;
            while(cnt < seriesECG->count() && (lastX - seriesECG->at(cnt).x()) > timeWindow)
                cnt++;
            if(cnt > 0)
                seriesECG->removePoints(0, cnt);

            if(!rPeak.empty())
            {
                qreal lastXRR = rPeak.back().time();
                cnt = 0;
                while(cnt < seriesRR->count() && (lastXRR - seriesRR->at(cnt).x()) > timeWindow)
                    cnt++;
                if(cnt > 0)
                {
                    seriesRR->removePoints(0, cnt);
                }
            }

            axisX()->setRange(lastX - timeWindow, lastX);
            scroll(lastX - firstX, 0.0);
        }
        qreal max = 0;
        qreal min = 0;
        qreal val;
        for(int idx = 0; idx < seriesECG->count(); idx++)
        {
            val = seriesECG->at(idx).y();
            max = (max < val) ? val : max;
            min = (min > val) ? val : min;
        }
        qreal dyScale = (max - min) * 0.2;
        axisY()->setRange(min - dyScale, max + dyScale);
    }
}
