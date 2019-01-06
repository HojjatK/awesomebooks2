import { Component, OnInit } from '@angular/core';
import { ChartService } from '../shared/services/chart/chart.service';
import { ChartResult, ChartSerie, ChartSerieResult } from '../shared/models';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  constructor(private chartService : ChartService) {  
  }

  public chartOptions = {
    responsive: true,
    responsiveAnimationDuration: 0,
    maintainAspectRatio: true,
    scales: {
      yAxes: [{
        stacked: false,
        gridLines: {
          display: true,
          color: "rgba(255,99,132,0.2)"
        }
      }],
      xAxes: [{
        gridLines: {
          display: false
        }
      }]
    }
  }

  // lineChart
  public lineChartLegend: boolean = true;
  public lineChartType: string = 'line';
  public lineChartColors: Array <any>= [
    { // grey
      backgroundColor: 'rgba(148,159,177,0.2)',
      borderColor: 'rgba(148,159,177,1)',
      pointBackgroundColor: 'rgba(148,159,177,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)'
    },
    { // dark grey
      backgroundColor: 'rgba(77,83,96,0.2)',
      borderColor: 'rgba(77,83,96,1)',
      pointBackgroundColor: 'rgba(77,83,96,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(77,83,96,1)'
    },
    { // grey
      backgroundColor: 'rgba(148,159,177,0.2)',
      borderColor: 'rgba(148,159,177,1)',
      pointBackgroundColor: 'rgba(148,159,177,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)'
    }
  ]; 
  public lineChartLabels: string[] = null;
  public lineChartData: Array<{data: Array<number>, label: string}> = null;  
  public lineChartOptions: any = {
    responsive: true
  };    
  

  // Bar Chart
  public barChartOptions: any = {
    scaleShowVerticalLines: false,
    responsive: true
  };
  public barChartType: string = 'bar';
  public barChartLegend: boolean = true;
  public barChartLabels: string[] = null;
  public barChartData: Array<{ data: Array<number>, label: string }> = null;

  // Doughnut
  public doughnutChartType: string = 'doughnut';
  public doughnutChartLabels: string[] = null;
  public doughnutChartData: number[] = null;  

  // Pie
  public pieChartType: string = 'pie';
  public pieChartLabels: string[] = null;
  public pieChartData: number[] = null;  

  // Radar
  public radarChartLabels: string[] = null;
  public radarChartData:  Array<{data: Array<number>, label: string}> = null;
  public radarChartType: string = 'radar';  

  // PolarArea
  public polarAreaChartType: string = 'polarArea';
  public polarAreaChartLabels: string[] = null;
  public polarAreaChartData: number[] = null;
  public polarAreaLegend: boolean = true;


  // events
  public chartClicked(e: any): void {
    console.log(e);
  }

  public chartHovered(e: any): void {
    console.log(e);
  }
  private categoryGroupChart : ChartResult;
  private categoryChart : ChartResult;
  private categorySerieChart : ChartSerieResult;
  private bookPublishChart : ChartSerieResult;
  

  ngOnInit() {
    this.chartService.getCategoryGroupChart().subscribe(res => {
      this.categoryGroupChart = res.data;

      this.doughnutChartLabels = this.categoryGroupChart.labels;
      this.doughnutChartData = this.categoryGroupChart.data;
    });

    this.chartService.getCategoryChart().subscribe(res => {
      this.categoryChart = res.data;

      this.pieChartLabels = this.categoryChart.labels;
      this.pieChartData = this.categoryChart.data;

      this.polarAreaChartLabels = this.categoryChart.labels;
      this.polarAreaChartData = this.categoryChart.data;
    });

    this.chartService.getCategorySerieChart().subscribe(res => {
      this.categorySerieChart = res.data;
      let series = this.categorySerieChart.data.map(function(serie) { return { data: serie.data, label: serie.serie_name }});

      this.radarChartLabels = this.categorySerieChart.labels;        
      this.radarChartData = series;
    });

    this.chartService.getBookPublishChart().subscribe(res => {
      this.bookPublishChart = res.data;
      let series = this.bookPublishChart.data.map(function(serie) { return { data: serie.data, label: serie.serie_name }});

      this.lineChartLabels = this.bookPublishChart.labels;        
      this.lineChartData = series;

      this.barChartLabels = this.bookPublishChart.labels;
      this.barChartData = series;
    });
  }

}
