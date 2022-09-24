<template>
  <div id="location_legend">
    <div class="loc_legend_part">
      <div class="loc_in loc_div_base"></div>
      <div class="loc_legend_text">
        <label class="status_text">待入</label>
      </div>
    </div>
    <div class="loc_legend_part">
      <div class="loc_out loc_div_base"></div>
      <div class="loc_legend_text">
        <label class="status_text">待出</label>
      </div>
    </div>
    <div class="loc_legend_part">
      <div class="loc_storged loc_div_base"></div>
      <div class="loc_legend_text">
        <label class="status_text">已存储</label>
      </div>
    </div>
    <div class="loc_legend_part">
      <div class="loc_forbidden loc_div_base">
        <img class="loc_forbidden" src="../assets/img/forbidden16.png" />
      </div>
      <div class="loc_legend_text">
        <label class="status_text">禁用</label>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "Legend",
  props: {},
  data() {
    return {};
  },

  mounted() {},
};
</script>

<style scoped>
.location_legend {
  width: 100%;
  height: 100%;
}
.status_text {
  vertical-align: middle;
  font-size: 10px;
}
.loc_legend_part {
  width: auto;
  height: auto;
  display: flex;
}
.loc_div_base {
  width: 15px;
  height: 15px;
}
.loc_forbidden {
  width: 15px;
  height: 15px;
}
.loc_storged {
  background-color: #409eff;
}
.loc_in {
  background-color: #67c23a;
}
.loc_out {
  background-color: #f56c6c;
}
.loc_legend_text {
  margin-left: 2%;
}
</style>
