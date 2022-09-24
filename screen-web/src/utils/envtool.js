function getConfigByEnv() {
  var pth =
    process.env.NODE_ENV === "development" ? "./config.json" : "../config.json";
  var request = new XMLHttpRequest();
  request.open("GET", pth, false);
  request.send(null);

  var config_json = JSON.parse(request.responseText);
  return config_json;
}

export default { getConfigByEnv };
