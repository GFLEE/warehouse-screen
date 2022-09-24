<template>
  <div id="app">
    <Screen />
  </div>
</template>

<script>
import Screen from "./components/Screen.vue";
import Vue from "vue";

import { connectServer } from "@/utils/signalR";

export default {
  name: "App",
  components: {
    Screen,
  },
  mounted() {
    var url = Vue.prototype.$config.ServiceUrl;
    connectServer(url, "virtual data");
  },
};
</script>

<style>
#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
  width: 100%;
  height: 100%;
}
</style>
