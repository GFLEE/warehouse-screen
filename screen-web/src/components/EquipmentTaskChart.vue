<template>
  <div id="c_container" />
</template>

<script>
import bus from "@/utils/bus";
import * as echarts from "echarts";
import Vue from "vue";
var v_type;

var myChart = null;

var inbound_tasks = [];
var outbound_tasks = [];
var colorList = ["rgb(6,196,236)", "rgb(48,130,222)"];
var font_size = 12;
export default {
  name: "EquipmentTaskChart",
  data() {
    return { eups: [] };
  },
  mounted() {
    v_type = Vue.prototype.$config.ZoneConfig == "ZJK" ? "正极库" : "负极库";
    var chartDom = document.getElementById("c_container");
    myChart = echarts.init(chartDom);

    bus.$on("equ_task", (data) => {
      this.eups = [];
      inbound_tasks = [];
      outbound_tasks = [];

      var data_arr = JSON.parse(data);

      data_arr = data_arr.filter((item) => item.sc.search(v_type) != -1);

      for (var k = 0, length = data_arr.length; k < length; k++) {
        var element = data_arr[k];
        this.eups.push(element.sc);
        inbound_tasks.push(element.in_task_count);
        outbound_tasks.push(element.out_task_count);
      }
      myChart.setOption({
        xAxis: [
          {
            type: "category",
            show: true,
            splitLine: {
              show: false, // 不显示网格线
            },
            data: this.eups,
            axisLabel: {
              show: true,
              interval: 0,
              color: "#F2F6FC",
              fontSize: font_size,
              formatter: function (params) {
                var newParamsName = "";
                var paramsNameNumber = params.length;
                var provideNumber = 3; // 一行显示几个字 然后就超过字数就会自动换行
                var rowNumber = Math.ceil(paramsNameNumber / provideNumber);
                if (paramsNameNumber > provideNumber) {
                  for (var p = 0; p < rowNumber; p++) {
                    var tempStr = "";
                    var start = p * provideNumber;
                    var end = start + provideNumber;
                    if (p == rowNumber - 1) {
                      tempStr = params.substring(start, paramsNameNumber);
                    } else {
                      tempStr = params.substring(start, end) + "\n";
                    }
                    newParamsName += tempStr;
                  }
                } else {
                  newParamsName = params;
                }
                return newParamsName;
              },
            },
          },
        ],
        yAxis: [
          {
            type: "value",
            minInterval: 1,
            splitLine: {
              show: false, // 不显示网格线
            },
            axisLabel: {
              show: true,
              color: "#F2F6FC",
              fontSize: font_size,
            },
          },
        ],
        color: colorList,
        legend: {
          data: ["入库任务", "出库任务"],
          itemWidth: 20,
          itemHeight: 10,
          textStyle: {
            color: "#F2F6FC",
            fontSize: font_size,
          },
        },
        tooltip: {
          trigger: "axis",
          axisPointer: {
            type: "cross",
            label: {
              backgroundColor: "#283b56",
            },
          },
        },
        series: [
          {
            name: "入库任务",
            type: "bar",
            itemStyle: {
              normal: {
                color: function (params) {
                  return colorList[0];
                },
              },
            },
            stack: "inbound",
            emphasis: {
              focus: "series",
            },
            data: inbound_tasks,
          },
          {
            name: "出库任务",
            type: "bar",
            itemStyle: {
              normal: {
                color: function (params) {
                  return colorList[1];
                },
              },
            },
            stack: "outbound",
            emphasis: {
              focus: "series",
            },
            data: outbound_tasks,
          },
        ],
      });
    });
  },
};
</script>

<style lang="less">
#c_container {
  width: 100%;
  height: 100%;
}
</style>
