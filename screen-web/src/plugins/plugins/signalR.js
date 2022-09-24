import bus from "../../utils/bus";
const signalR = require("@aspnet/signalr");
// Vue.prototype.sendMsg = signalR.sendMsg;

let connection = null;

export let connectServer = (url, conn_data) => {
  console.log("开始连接.....");
  connection = new signalR.HubConnectionBuilder()
    .withUrl(url)
    .configureLogging(signalR.LogLevel.Information)
    .build();
  //初始化
  initSocket(conn_data);
};
//初始化包括 连接，获取消息
let initSocket = () => {
  //连接
  connection
    .start()
    .then(function () {
      console.log("已连接，connection.start()执行");
    })
    .catch(function (err) {
      return console.error(err.toString());
    });
};

let endConnect = () => {
  connection.stop(() => {});
};
export default { connectServer, endConnect, connection };
