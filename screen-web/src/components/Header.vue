<template>
  <div id="top-header">
    <dv-decoration-8 class="header-left-decoration" />
    <dv-decoration-5 class="header-center-decoration" />
    <dv-decoration-8 class="header-right-decoration" :reverse="true" />
    <div class="center-title">{{ v_type }}</div>
    <!--<div class="mh-right">
      <dv-border-box-12 class="datebox"> {{ datetime }} </dv-border-box-12>
    </div> -->
  </div>
</template>

<script>
import Vue from "vue";
export default {
  name: "Header",
  data() {
    return { v_type: "" };
  },
  mounted() {
    this.v_type =
      Vue.prototype.$config.ZoneConfig == "ZJK"
        ? "正极原材料库"
        : "负极原材料库";
  },
};
</script>

<style lang="less">
#top-header {
  position: relative;
  width: 100%;
  height: 80px;
  display: flex;
  justify-content: space-between;
  flex-shrink: 0;

  .header-center-decoration {
    width: 40%;
    height: 60px;
    margin-top: 30px;
  }

  .header-left-decoration,
  .header-right-decoration {
    width: 25%;
    height: 60px;
  }
  @font-face {
    font-family: "digital-7";
    src: url("../assets/fonts/digital-7.ttf") format("truetype");
  }
  .center-title {
    position: absolute;
    font-weight: bold;
    left: 50%;
    top: 15px;
    color: #b4e4ff;
    font-family: "digital-7";
    font-size: 30px;
    transform: translateX(-50%);
  }
  .datebox {
    font-size: 30px;
    color: #b4e4ff;
    font-family: "digital-7";
    width: auto;
    height: 50px;
    top: 15px;
    line-height: 50px;
    text-align: center;
    margin-left: 200px;
  }
  .mh-right {
    width: 450px;
  }
}
</style>
