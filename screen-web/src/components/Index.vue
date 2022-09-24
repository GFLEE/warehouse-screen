<template>
  <div id="data-view">
    <dv-full-screen-container>
      <div class="main-header"><Header /></div>
      <dv-border-box-1 class="main-container">
        <div class="left-chart-container"><LeftPart></LeftPart></div>
        <div class="right-main-container">
          <div class="rmc-top-container">
            <dv-border-box-13 class="rmctc-left-container">
              <Location />
            </dv-border-box-13>
          </div>
        </div>
      </dv-border-box-1>
    </dv-full-screen-container>
  </div>
</template>

<script>
import Header from "./Header.vue";
import Location from "./Location.vue";
import LeftPart from "./LeftPart.vue";

export default {
  name: "datav",
  components: {
    Header,
    Location,
    LeftPart,
  },
  data() {
    return {};
  },
  methods: {},
};
</script>

<style lang="less">
.TaskItem {
  margin-top: 7%;
}
.LaneWayEmpty {
  width: auto;
}
.inventory-storged {
  margin-top: 3%;
}
#data-view {
  width: 100%;
  height: 100%;
  background-color: #030409;
  color: #fff;

  #dv-full-screen-container {
    background-image: url("./img/bg.png");
    background-size: 100% 100%;
    box-shadow: 0 0 3px blue;
    display: flex;
    flex-direction: column;
  }

  .main-header {
    height: 8%;
    display: flex;
    justify-content: space-between;
    align-items: flex-end;

    .mh-left {
      font-size: 20px;
      color: rgb(1, 134, 187);

      a:visited {
        color: rgb(1, 134, 187);
      }
    }

    .mh-middle {
      font-size: 30px;
    }

    .mh-left,
    .mh-right {
      width: 450px;
    }
  }

  .main-container {
    height: calc(~"100% - 80px");

    .border-box-content {
      padding: 20px;
      box-sizing: border-box;
      display: flex;
    }
  }

  .left-chart-container {
    width: 25%;
    box-sizing: border-box;

    .border-box-content {
      flex-direction: column;
    }
  }

  .right-main-container {
    width: 100%;
    padding-left: 2px;
    box-sizing: border-box;
  }

  .rmc-top-container {
    height: 100%;
    display: flex;
  }

  .rmctc-left-container {
    width: 100%;
  }

  .rmctc-right-container {
    width: 35%;
  }

  .rmc-bottom-container {
    height: 35%;
  }

  .rmctc-chart-1,
  .rmctc-chart-2 {
    height: 50%;
  }
}
</style>
