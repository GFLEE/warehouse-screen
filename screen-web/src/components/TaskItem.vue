<template>
  <grid-layout
    :layout.sync="layout"
    :col-num="12"
    :row-height="30"
    :is-draggable="draggable"
    :is-resizable="resizable"
    :vertical-compact="true"
    :use-css-transforms="true"
  >
    <grid-item
      class="digital-flop-item"
      v-for="item in layout"
      :key="item.i"
      :static="item.static"
      :x="item.x"
      :y="item.y"
      :w="item.w"
      :h="item.h"
      :i="item.i"
    >
      <div class="digital-flop-title" style="font-size: 10px">
        {{ item.title }}
      </div>

      <div class="digital-flop">
        <dv-digital-flop
          :config="item.config"
          style="width: 80px; height: 50px"
        />
      </div>
    </grid-item>
  </grid-layout>
</template>

<script>
import bus from "@/utils/bus";
import { GridLayout, GridItem } from "vue-grid-layout";
const colors = ["rgb(170,194,223)", "#40faee", "#4d99fc", "#f46827"];
import Vue from "vue";
var v_type;

export default {
  name: "TaskItem",
  components: { GridLayout, GridItem },
  data() {
    return {
      layout: [
        {
          x: 0,
          y: 0,
          w: 6,
          h: 2,
          i: "1",
          static: true,
          title: "",
          config: {},
        },
        {
          x: 6,
          y: 0,
          w: 6,
          h: 2,
          i: "2",
          static: true,
          title: "",
          config: {},
        },
        { x: 0, y: 2, w: 4, h: 2, i: "3", static: true, title: "", config: {} },
        { x: 6, y: 2, w: 4, h: 2, i: "4", static: true, title: "", config: {} },
      ],
      draggable: true,
      resizable: true,
      index: 0,
    };
  },
  methods: {},
  mounted() {
    v_type = Vue.prototype.$config.ZoneConfig;

    bus.$on("task_item", (data) => {
      var array = JSON.parse(data);
      array = array.filter((item) => item.zone_code.search(v_type) != -1);
      for (var k = 0, length = array.length; k < length; k++) {
        var element = array[k];
        var count_intger = parseInt(element.count);
        var item_config = {
          number: [count_intger],
          content: "{nt}",
          textAlign: "center",
          style: {
            fill: colors[0],
            fontWeight: "bold",
            fontSize: 20,
          },
        };

        this.layout[k].config = item_config;
        this.layout[k].title = element.title;
      }
    });
  },
};
</script>

<style lang="less">
#digital-flop {
  position: relative;
  height: 15%;
  flex-shrink: 0;
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: rgba(6, 30, 93, 0.5);
  @font-face {
    font-family: "digital-7";
    src: url("../assets/fonts/digital-7.ttf") format("truetype");
  }
  .digital-flop-item {
    width: 11%;
    height: 80%;
    display: flex;
    flex-direction: column;
    justify-content: center;
    // font-family: "digital-7";
    align-items: center;
    border-left: 3px solid rgb(6, 30, 93);
    border-right: 3px solid rgb(6, 30, 93);
  }

  .digital-flop-title {
    margin-bottom: 20px;
  }
  .title_font {
    font-size: 10px;
  }
  .header-item {
    font-size: 10px;
  }
  .digital-flop {
    display: flex;
  }

  .unit {
    margin-left: 5px;
    display: flex;
    align-items: flex-end;
    box-sizing: border-box;
    padding-bottom: 13px;
  }

  .vue-grid-layout {
    background: #eee;
  }
  .vue-grid-item:not(.vue-grid-placeholder) {
    background: #ccc;
    border: 1px solid black;
  }
  .vue-grid-item .resizing {
    opacity: 0.9;
  }
  .vue-grid-item .static {
    background: #cce;
  }
  .vue-grid-item .text {
    font-size: 15px;
    text-align: center;
    position: absolute;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    margin: auto;
    // height: 100%;
    // width: 100%;
  }
  .vue-grid-item .no-drag {
    height: 100%;
    width: 100%;
  }
  .vue-grid-item .minMax {
    font-size: 12px;
  }
  .vue-grid-item .add {
    cursor: pointer;
  }
  .vue-draggable-handle {
    position: absolute;
    width: 20px;
    height: 20px;
    top: 0;
    left: 0;
    background: url("data:image/svg+xml;utf8,<svg xmlns='http://www.w3.org/2000/svg' width='10' height='10'><circle cx='5' cy='5' r='5' fill='#999999'/></svg>")
      no-repeat;
    background-position: bottom right;
    padding: 0 8px 8px 0;
    background-repeat: no-repeat;
    background-origin: content-box;
    box-sizing: border-box;
    cursor: pointer;
  }
}
</style>
