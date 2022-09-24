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

import Vue from "vue";
const v_zone = Vue.prototype.$config.ZoneConfig; 
const v_url = Vue.prototype.$config.ServiceUrl;

var engine = null,
  container = null;
let temp = "hello";
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
        40,
        container.clientWidth / container.clientHeight,
        1,
        1000
      );

      // camera.position.set(118, 152, 31);
      camera.position.set(
        150.95653708068764,
        342.10070121809224,
        -296.113946276356074
      );

      // camera.rotation.order = "YXZ";
      // camera.rotation.x = -0.5016486840888801;
      // camera.rotation.y = -2.3467159419520294;
      // camera.rotation.z = -6.331172936914994e-17;

      renderer = new THREE.WebGLRenderer({ alpha: true });
      renderer.setSize(container.clientWidth, container.clientHeight);
      document.getElementById("container").appendChild(renderer.domElement);

      //===================================================

      controls = new OrbitControls(camera, container);
    },
    animate() {
      // console.log(camera);
      let time = Date.now();
      let t = (time % loopTime) / loopTime; // 计算当前时间进度百分比

      this.changePosition(t);
      this.changeLookAt(t);

      requestAnimationFrame(this.animate);

      controls.update();
      this.renderAni();
    },
    renderAni() {
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
      // console.log(data_arr);
      var axes = new THREE.AxesHelper(300);
      scene.add(axes);
      var grid = new THREE.GridHelper(500, 50, 0x2c2c2c, 0x888888);
      scene.add(grid);

      this.renderCurveTest();
      // var geo = new THREE.PlaneBufferGeometry(400, 400);
      // var mat = new THREE.MeshBasicMaterial({
      //   color: "rgb(48, 130, 222)",
      //   side: THREE.DoubleSide,
      // });
      // var plane = new THREE.Mesh(geo, mat);
      // plane.rotateX( - Math.PI / 2);
      // scene.add(plane);

      // //===========s摄像机===============
      // var camera_geometry = new THREE.BoxBufferGeometry(5, 5, 5);
      // var camera_material = new THREE.MeshBasicMaterial({ color: 0xffff00 });
      // var camera_cube = new THREE.Mesh(camera_geometry, camera_material);
      // camera_cube.position.copy(camera.position);
      // camera_cube.rotation.copy(camera.rotation);

      // scene.add(camera_cube);
      //==========================
      var loader = new THREE.TextureLoader();
      var cube;
      var forbidden_img;
      var Texturing = require("../assets/img/forbidden.png");
      forbidden_img = loader.load(Texturing);

      var wire_cube = new THREE.Mesh(
        new THREE.BoxGeometry(cube_width, cube_width, cube_width),
        new THREE.MeshBasicMaterial({
          color: "#FFFFFF",
          side: THREE.BackSide,
          wireframe: true,
        })
      );

      var arr_length = data_arr.length;
      for (var k = 0, length = arr_length; k < length; k++) {
        var element = data_arr[k];

        var x = parseInt(element.location_row);
        var y = parseInt(element.location_layer);
        var z = parseInt(element.location_column);
        var row_cord, column_cord, z_column;
        if (v_zone == "ZJK") {
          row_cord = 6 * x - 5;
          column_cord = y * cube_width;
          z_column = z * cube_width;
        }
        if (v_zone == "FJK") {
          row_cord = 6 * x - 5 - 80;
          column_cord = y * cube_width;
          z_column = z * cube_width - 50;
        }
        var location_flag = element.location_flag;

        if (element.location_status == "LocationStatus_Empty") {
          cube = new THREE.Mesh(
            new THREE.BoxGeometry(cube_width, cube_width, cube_width),
            new THREE.MeshBasicMaterial({
              color: "#FFFFFF",
              side: THREE.BackSide,
              wireframe: true,
            })
          );
          cube.renderOrder = 5;

          cube.scale.x = cube_scale;
          cube.scale.y = cube_scale;
          cube.scale.z = cube_scale;

          cube.position.x = row_cord * cube_width;
          cube.position.y = column_cord;
          cube.position.z = z_column;

          scene.add(cube);
        }

        if (element.location_status == "LocationStatus_Stored") {
          cube = new THREE.Mesh(
            new THREE.BoxGeometry(cube_width, cube_width, cube_width),
            new THREE.MeshBasicMaterial({
              color: "#409EFF",
            })
          );
          if (location_flag == "LocationFlag_Forbidden") {
            cube = new THREE.Mesh(
              new THREE.BoxGeometry(cube_width, cube_width, cube_width),
              new THREE.MeshBasicMaterial({
                map: forbidden_img,
                transparent: false,
                opacity: 0.8,
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
            //=================
            // var bx = new THREE.BoxGeometry(cube_width, cube_width, cube_width);
            // bx.location_no = element.location_no;
            // var hex = 0x409eff;

            // bx.addAttribute("color", new THREE.BufferAttribute(hex, 3));

            // var geo = new THREE.EdgesGeometry(bx);
            // var mat = new THREE.LineBasicMaterial({
            //   color: "#409EFF",
            //   linewidth: 4,
            // });
            // var cube3 = new THREE.LineSegments(geo, mat);
            // cube3.renderOrder = 1; // make sure wireframes are rendered 2nd

            //==================
            cube = new THREE.Mesh(
              new THREE.BoxGeometry(cube_width, cube_width, cube_width),
              new THREE.MeshBasicMaterial({
                color: "#409EFF",
              })
            );
            cube.scale.x = cube_scale;
            cube.scale.y = cube_scale;
            cube.scale.z = cube_scale;

            cube.position.x = row_cord * cube_width;
            cube.position.y = column_cord;
            cube.position.z = z_column;
            scene.add(cube);

            //===============画边框====
            wire_cube.renderOrder = 5;
            wire_cube.scale.x = cube_scale;
            wire_cube.scale.y = cube_scale;
            wire_cube.scale.z = cube_scale;

            wire_cube.position.x = row_cord * cube_width;
            wire_cube.position.y = column_cord;
            wire_cube.position.z = z_column;

            scene.add(wire_cube);
            //========================
          }
        }

        if (element.location_status == "LocationStatus_In") {
           
          cube = new THREE.Mesh(
            new THREE.BoxGeometry(cube_width, cube_width, cube_width),
            new THREE.MeshBasicMaterial({
              color: "#67C23A",
            })
          );
          cube.scale.x = cube_scale;
          cube.scale.y = cube_scale;
          cube.scale.z = cube_scale;
          //==================
          cube.renderOrder = 2; // make sure wireframes are rendered 2nd
          cube.position.x = row_cord * cube_width;
          cube.position.y = column_cord;
          cube.position.z = z_column;
          scene.add(cube);
        }

        if (element.location_status == "LocationStatus_Out") {
          cube = new THREE.Mesh(
            new THREE.BoxGeometry(cube_width, cube_width, cube_width),
            new THREE.MeshBasicMaterial({
              color: "#F56C6C",
            })
          );
          cube.scale.x = cube_scale;
          cube.scale.y = cube_scale;
          cube.scale.z = cube_scale;

          cube.renderOrder = 2; // make sure wireframes are rendered 2nd
          cube.position.x = row_cord * cube_width;
          cube.position.y = column_cord;
          cube.position.z = z_column;
          scene.add(cube);
        }
      }
    },
  },
  mounted() {
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
.div-taskchart {
  margin: 0 0 auto auto;
  width: 35%;
  height: 40%;
  z-index: 9;
}
.div-inventorychart {
  margin: auto 0 0 auto;
  margin-top: 14%;
  width: 37%;
  height: 40%;
  z-index: 9;
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
