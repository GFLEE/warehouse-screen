<template>
  <div id="left_part_container">
    <dv-border-box-8>
      <LaneWayEmpty class="LaneWayEmpty" />
      <TaskItem class="TaskItem" />
      <InVentoryStorged class="inventory-storged" />
      <DateTime />
    </dv-border-box-8>
  </div>
</template>

<script>
import { connectServer } from "@/utils/signalR";
import bus from "@/utils/bus";
// import LaneWayEmpty from "./LaneWayEmpty.vue";
import LaneWayEmpty from "./NewLaneWayEmpty.vue";

import TaskItem from "./TaskItem.vue";
import DateTime from "./DateTime.vue";
import InVentoryStorged from "./InVentoryStorged.vue";
 
export default {
  name: "LeftPart",
  components: {
    LaneWayEmpty,
    TaskItem,
    DateTime,
    InVentoryStorged,
  },

  data() {
    return {};
  },

  mounted() {},
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
#left_part_container {
  width: 100%;
  height: 100%;
}
.left_part_border {
  width: 100%;
  height: 100%;
}
</style>
