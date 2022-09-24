<template>
  <div class="parent">
    <div id="container"></div>
    <div class="div-taskchart">
      <dv-border-box-12><EquipmentTaskChart /></dv-border-box-12>
    </div>
    <div class="div-inventorychart">
      <dv-border-box-12><InventoryDist /></dv-border-box-12>
    </div>
    <div class="div-location-legend">
      <Legend />
    </div>
  </div>
</template>

<script>
import bus from "@/utils/bus";
import * as THREE from "three";
import { OrbitControls } from "three/examples/jsm/controls/OrbitControls";
import * as Vis from "vis-three";
import Stats from "three/examples/jsm/libs/stats.module.js";
import EquipmentTaskChart from "./EquipmentTaskChart.vue";
import InventoryDist from "./InventoryDist.vue";
import Legend from "./Legend.vue";
// import LeftPart from "./LeftPart.vue";

import Vue from "vue";

var v_zone, v_url;
var engine = null,
  container = null;
var camera, scene, renderer, controls;
var sphere, cube;
var cube_width = 5;
var cube_scale = 0.8;
const loopTime = 10 * 1000; // loopTime: 循环一圈的时间
var curve, stacker, position;
export default {
  name: "Location",
  components: {
    EquipmentTaskChart,
    InventoryDist,
    Legend,
    // LeftPart,
  },
  props: {
    msg: String,
  },
  data() {
    return {
      fjk_data: [],
      spinning: false,
      zjk_data: [],
    };
  },
  methods: {
    init() {
      container = document.getElementById("container");
      scene = new THREE.Scene();
      camera = new THREE.PerspectiveCamera(
        23,
        container.clientWidth / container.clientHeight,
        1,
        10000
      );
      if (v_zone == "ZJK") {
        camera.position.set(
          -385.8191699788333,
          384.5775420101582,
          -5.45260109503206
        );
      }
      if (v_zone == "FJK") {
        camera.position.set(
          -439.4465496292775,
          545.8738579058396,
          30.80726493894258
        );
      }
      renderer = new THREE.WebGLRenderer({ alpha: true });
      renderer.setSize(container.clientWidth, container.clientHeight);
      document.getElementById("container").appendChild(renderer.domElement);

      //===================================================

      controls = new OrbitControls(camera, container);
    },
    animate() {
      // let now_t = Date.now();
      // let t = (now_t % loopTime) / loopTime; // 计算当前时间进度百分比
      // this.changePosition(t);
      // this.changeLookAt(t);

      requestAnimationFrame(this.animate);
      controls.update();
      renderer.render(scene, camera);
    },

    renderCurveTest() {
      var initialPoints = [];
      if (v_zone == "ZJK") {
        initialPoints = [
          { x: 20, y: 5, z: 5 },
          { x: 20, y: 5, z: 180 },
        ];
      }
      var cubeList = [];
      initialPoints.map((pos) => {
        const geometry = new THREE.BoxBufferGeometry(5, 5, 5);
        const material = new THREE.MeshBasicMaterial({ color: 0xffff00 });
        const cube = new THREE.Mesh(geometry, material);
        cube.position.copy(pos);
        cubeList.push(cube);
        scene.add(cube);
      });

      stacker = new THREE.Mesh(
        new THREE.BoxBufferGeometry(5, 5, 5),
        new THREE.MeshBasicMaterial({ color: 0xf2f6fc })
      );
      stacker.position.copy({ x: 20, y: 5, z: 32 });
      scene.add(stacker);

      curve = new THREE.CatmullRomCurve3(
        cubeList.map((cube) => cube.position) // 直接绑定方块的position以便后续用方块调整曲线
      );
      curve.curveType = "chordal"; // 曲线类型
      curve.closed = true; // 曲线是否闭合

      const points = curve.getPoints(50); // 50等分获取曲线点数组
      const line = new THREE.LineLoop(
        new THREE.BufferGeometry().setFromPoints(points),
        new THREE.LineBasicMaterial({ color: 0x00ff00 })
      ); // 绘制实体线条，仅用于示意曲线，后面的向量线条同理，相关代码就省略了

      scene.add(line);
    },

    changePosition(t) {
      if (curve == undefined) {
        return;
      }
      position = curve.getPointAt(t); // t: 当前点在线条上的位置百分比，后面计算
      stacker.position.copy(position);
    },
    changeLookAt(t) {
      if (curve == undefined) {
        return;
      }
      const tangent = curve.getTangentAt(t);
      const lookAtVec = tangent.add(position); // 位置向量和切线向量相加即为所需朝向的点向量
      stacker.lookAt(lookAtVec);
    },
    renderLocation(data_arr) {
      scene = new THREE.Scene();

      // this.renderCurveTest();

      // var axes = new THREE.AxesHelper(300);
      // scene.add(axes);
      // var grid = new THREE.GridHelper(500, 50, 0x2c2c2c, 0x888888);
      // scene.add(grid);
      // var geo = new THREE.PlaneBufferGeometry(400, 400);
      // var mat = new THREE.MeshBasicMaterial({
      //   color: "rgb(48, 130, 222)",
      //   side: THREE.DoubleSide,
      // });
      // var plane = new THREE.Mesh(geo, mat);
      // plane.rotateX( - Math.PI / 2);
      // scene.add(plane);
      var arr_length = data_arr.length;
      var loader = new THREE.TextureLoader();
      var cube;
      var forbidden_img;
      var Texturing = require("../assets/img/forbidden128_st.png");
      var Texturing_empty = require("../assets/img/forbidden48.png");
      forbidden_img = loader.load(Texturing);
      var empty_forbidden_img = loader.load(Texturing_empty);
      for (var k = 0, length = arr_length; k < length; k++) {
        var element = data_arr[k];
        var location_flag = element.location_flag;

        var x = parseInt(element.location_row);
        var y = parseInt(element.location_layer);
        var z = parseInt(element.location_column);
        var row_cord, column_cord, z_column;
        if (v_zone == "ZJK") {
          row_cord = 6 * x - 5 - 20;
          column_cord = y * cube_width;
          z_column = z * cube_width - 140;
        }
        if (v_zone == "FJK") {
          row_cord = 6 * x - 5 - 70;
          column_cord = y * cube_width;
          z_column = z * cube_width - 180;
        }

        if (element.location_status == "LocationStatus_Empty") {
          if (location_flag == "LocationFlag_Forbidden") {
            cube = new THREE.Mesh(
              new THREE.BoxGeometry(cube_width, cube_width, cube_width),
              new THREE.MeshBasicMaterial({
                map: empty_forbidden_img,
                // transparent: true,
                opacity: 0.7,
              })
            );

            cube.scale.x = cube_scale;
            cube.scale.y = cube_scale;
            cube.scale.z = cube_scale;

            cube.position.x = row_cord * cube_width;
            cube.position.y = column_cord;
            cube.position.z = z_column;

            scene.add(cube);
          } else {
            // var line_l = new THREE.Mesh(
            //   new THREE.BoxGeometry(cube_width, cube_width, cube_width),
            //   new THREE.MeshBasicMaterial({
            //     color: "#E6E8FA ",
            //     side: THREE.BackSide,
            //     wireframe: true,
            //   })
            // );
            // line_l.renderOrder = 5;

            const geometry = new THREE.BoxBufferGeometry(
              cube_width,
              cube_width,
              cube_width
            );
            const material = new THREE.MeshBasicMaterial({
              color: "rgb(64,150,228)",
              transparent: true,
              opacity: 0.4,
              // side: THREE.DoubleSide,
              // alphaTest: 0.5,
            });
            const line_l = new THREE.Mesh(geometry, material);

            line_l.scale.x = cube_scale;
            line_l.scale.y = cube_scale;
            line_l.scale.z = cube_scale;

            line_l.position.x = row_cord * cube_width;
            line_l.position.y = column_cord;
            line_l.position.z = z_column;

            scene.add(line_l);
          }
        }

        if (element.location_status == "LocationStatus_Stored") {
          if (location_flag == "LocationFlag_Forbidden") {
            cube = new THREE.Mesh(
              new THREE.BoxGeometry(cube_width, cube_width, cube_width),
              new THREE.MeshBasicMaterial({
                map: forbidden_img,
                transparent: false,
                opacity: 1,
              })
            );
            cube.scale.x = cube_scale;
            cube.scale.y = cube_scale;
            cube.scale.z = cube_scale;

            cube.position.x = row_cord * cube_width;
            cube.position.y = column_cord;
            cube.position.z = z_column;

            scene.add(cube);
          } else {
            const cube3 = new THREE.Mesh(
              new THREE.BoxGeometry(cube_width, cube_width, cube_width),
              new THREE.MeshBasicMaterial({
                color: "#409EFF",
              })
            );
            cube3.scale.x = cube_scale;
            cube3.scale.y = cube_scale;
            cube3.scale.z = cube_scale;

            cube3.position.x = row_cord * cube_width;
            cube3.position.y = column_cord;
            cube3.position.z = z_column;

            scene.add(cube3);
          }
        }

        if (element.location_status == "LocationStatus_In") {
          const cube3 = new THREE.Mesh(
            new THREE.BoxGeometry(cube_width, cube_width, cube_width),
            new THREE.MeshBasicMaterial({
              color: "#67C23A",
            })
          );
          cube3.scale.x = cube_scale;
          cube3.scale.y = cube_scale;
          cube3.scale.z = cube_scale;
          //==================
          cube3.renderOrder = 2; // make sure wireframes are rendered 2nd
          cube3.position.x = row_cord * cube_width;
          cube3.position.y = column_cord;
          cube3.position.z = z_column;
          scene.add(cube3);
        }

        if (element.location_status == "LocationStatus_Out") {
          const cube3 = new THREE.Mesh(
            new THREE.BoxGeometry(cube_width, cube_width, cube_width),
            new THREE.MeshBasicMaterial({
              color: "#F56C6C",
            })
          );
          cube3.scale.x = cube_scale;
          cube3.scale.y = cube_scale;
          cube3.scale.z = cube_scale;

          cube3.renderOrder = 2; // make sure wireframes are rendered 2nd
          cube3.position.x = row_cord * cube_width;
          cube3.position.y = column_cord;
          cube3.position.z = z_column;
          scene.add(cube3);
        }
      }
    },
  },
  mounted() {
    v_zone = Vue.prototype.$config.ZoneConfig;
    v_url = Vue.prototype.$config.ServiceUrl;
    this.init();
    this.animate();

    if (v_zone == "FJK") {
      bus.$on("fjk_data", (data) => {
        this.renderLocation(JSON.parse(data));
      });
    }
    if (v_zone == "ZJK") {
      bus.$on("zjk_data", (data) => {
        this.renderLocation(JSON.parse(data));
      });
    }
  },
};
</script>

<style>
.parent {
  width: 100%;
  height: 100%;
  position: relative;
}
.LeftPart {
  width: 15%;
  height: 100%;
  z-index: 9;
  left: 0;
  top: 0;
  position: absolute;
}
.div-taskchart {
  margin: 0 0 auto auto;
  width: 35%;
  height: 47%;
  z-index: 9;
}
.div-inventorychart {
  width: 35%;
  height: 49%;
  z-index: 9;
  right: 0;
  bottom: 0;
  position: absolute;
}
.div-location-legend {
  position: absolute;
  left: 0;
  top: 0;
  width: 20%;
  height: 10%;
  z-index: 9;
}
#container {
  width: 100%;
  height: 100%;
  position: absolute;
  top: 0;
  left: 0;
  background-size: 100% 100%;
  background-repeat: no-repeat;
  box-shadow: 0 0 3px blue;
  background-position: 0 100%;
  display: flex;
  flex-direction: column;
}
</style>
