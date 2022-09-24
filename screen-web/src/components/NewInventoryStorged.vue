<template>
  <div class="storged-scroll-board">
    <ScrollTable />
  </div>
</template>

<script>
import ScrollTable from "./custom/ScrollTable.vue";
import Vue from "vue";
var v_type;

export default {
  name: "NewInventoryStorged",
  components: {
    ScrollTable,
  },
  data() {
    return {};
  },
  methods: {},
  mounted() {},
};
</script>

<style lang="less">
#scroll-board {
  width: 100%;
  box-sizing: border-box;
  height: 30%;
  overflow: hidden;
}
.span_auto {
  font-size: 5px;
}
</style>
