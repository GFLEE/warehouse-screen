import bus from "../utils/bus";
const signalR = require("@aspnet/signalr");
// Vue.prototype.sendMsg = signalR.sendMsg;

let connection = null;

export let connectServer = (url, conn_data) => {
  console.log("开始连接.....");
  connection = new signalR.HubConnectionBuilder()
    .withUrl(url)
    .configureLogging(signalR.LogLevel.Information)
    .build();
  initSocket(conn_data);
};
let initSocket = () => {
  connection
    .start()
    .then(function () {
      console.log("已连接，connection.start()执行");
    })
    .catch(function (err) {
      return console.error(err.toString());
    });

  connection.on("fjk_data", function (data1, data) {
    bus.$emit("fjk_data", data);
  });

  connection.on("zjk_data", function (data1, data) {
    bus.$emit("zjk_data", data);
  });

  connection.on("laneway_empty", function (data1, data) {
    bus.$emit("laneway_empty", data);
  });

  connection.on("task_item", function (data1, data) {
    bus.$emit("task_item", data);
  });
  connection.on("equ_task", function (data1, data) {
    bus.$emit("equ_task", data);
  });
  connection.on("inventory_data_zjk", function (data1, data) {
    bus.$emit("inventory_data_zjk", data);
  });
  connection.on("inventory_data_fjk", function (data1, data) {
    bus.$emit("inventory_data_fjk", data);
  });
  connection.on("screen_storged", function (data1, data) {
    bus.$emit("screen_storged", data);
  });
};

let endConnect = () => {
  connection.stop(() => {});
};
export default { connectServer, endConnect, connection };
