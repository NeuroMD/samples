#ifndef CHART_H
#define CHART_H


#include <QtCharts/QChart>
#include <QtCharts/QLineSeries>
#include <QtCharts/QSplineSeries>
#include <QtCharts/QBarSet>
#include <QtCore/QTimer>
#include "method/r_peak.h"
#include <QMetaType>

typedef std::vector<QPointF> QListPointF_t;
typedef std::vector<RPeak> RPeakPoint_t;

Q_DECLARE_METATYPE(QListPointF_t) // !!!!
Q_DECLARE_METATYPE(RPeakPoint_t) // !!!!

QT_CHARTS_BEGIN_NAMESPACE
class QSplineSeries;
class QValueAxis;
QT_CHARTS_END_NAMESPACE

QT_CHARTS_USE_NAMESPACE


class Chart: public QChart
{
    Q_OBJECT

public:
    Chart(QGraphicsItem *parent = 0, Qt::WindowFlags wFlags = 0);
    virtual ~Chart();
    void renderDeviceData(const QListPointF_t&, const RPeakPoint_t&);

private:
    const double timeWindow = 10.0;
    QLineSeries *seriesRR;
    QSplineSeries *seriesECG;
    QStringList titles;
    QValueAxis *axis;

};

#endif // CHART_H
