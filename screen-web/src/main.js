import Vue from "vue";
import App from "./App.vue";
import Antd from "ant-design-vue";
import Plugins from "@/plugins";
import dataV from "@jiaminghi/data-view";
import "./assets/common.less";
import * as echarts from "echarts";
import dayjs from "dayjs";
import envtool from "./utils/envtool.js";


var configData = envtool.getConfigByEnv();
 
Vue.prototype.$config = configData;
 
 
Vue.use(Plugins);
Vue.use(dataV);

Vue.use(Antd);

Vue.config.productionTip = false;
Vue.prototype.$echarts = echarts;
Vue.prototype.dayjs = dayjs;

new Vue({
  render: (h) => h(App),
}).$mount("#app");
