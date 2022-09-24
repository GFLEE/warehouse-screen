<template>
  <div id="inventory_container">
    <!-- <dv-active-ring-chart
      :config="config"
      style="width: 300px; height: 300px"
    />-->
  </div>
</template>

<script>
import bus from "@/utils/bus";
import * as echarts from "echarts";
import Vue from "vue";
var v_type;

var myChart = null;
var option;
var color = [
  "rgb(29,137,242)",
  "rgb(77,241,241)",
  "rgb(143,144,209)",
  "rgb(25,156,226)",
  "rgb(185,198,189)",
];
var font_size = 12;

export default {
  name: "InventoryDist",
  data() {
    return {};
  },

  mounted() {
    v_type = Vue.prototype.$config.ZoneConfig;
    var chartDom = document.getElementById("inventory_container");
    myChart = echarts.init(chartDom, null);

    if (v_type == "ZJK") {
      bus.$on("inventory_data_zjk", (data) => {
        this.initPie(JSON.parse(data));
      });
    }
    if (v_type == "FJK") {
      bus.$on("inventory_data_fjk", (data) => {
        this.initPie(JSON.parse(data));
      });
    }
  },
  methods: {
    initPie(data_arr) {
      var pie_arr = [];
      for (var k = 0, length = data_arr.length; k < length; k++) {
        var element = data_arr[k];
        pie_arr.push({ value: element.count, name: element.title });
      }

      option = {
        title: {
          text: "存储状态",
          left: "center",
          top: "center",
          textStyle: {
            color: "#F2F6FC",
            fontWeight: "normal",
            fontSize: font_size,
          },
        },
        series: [
          {
            type: "pie",
            color: color,
            itemStyle: {
              borderRadius: 5,
              borderColor: "#2c3e50",
              borderWidth: 2,
            },
            data: pie_arr,
            radius: ["40%", "70%"],
            label: {
              normal: {
                show: true,
                position: "outer",
                edgeDistance: "1%",
                bleedMargin: 10,
                formatter: "{b} : {c}",
                color: "#F2F6FC",
                fontWeight: "normal",
                fontSize: font_size,
              },
            },
          },
        ],
      };
      myChart.setOption(option);
      // window.onresize = function () {
      //   myChart.resize(200, 150);
      // };
    },
  },
};
</script>

<style lang="less">
#inventory_container {
  width: 100%;
  height: 100%;
}
</style>
