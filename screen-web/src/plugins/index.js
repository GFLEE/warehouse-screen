import hlightjsPlugin from "./plugins/highlight";
import signalRPlugin from "./plugins/signalR";
import JsonViewer from "./plugins/jsonViewer";

const Plugins = {
  install: function(Vue) {
    Vue.use(hlightjsPlugin);
    Vue.use(signalRPlugin);
    Vue.use(JsonViewer);
  }
};
export default Plugins;
