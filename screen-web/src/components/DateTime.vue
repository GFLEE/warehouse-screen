<template>
  <dv-border-box-1 class="datebox">{{ datetime }}</dv-border-box-1>
</template>

<script>
export default {
  name: "DateTime",
  props: {},
  data() {
    return { timer: "", datetime: "" };
  },

  mounted() {
    this.timer = setInterval(() => {
      this.datetime = this.dayjs().format("YYYY-MM-DD HH:mm:ss");
    }, 1000);
  },
  beforeDestroy() {
    if (this.timer) {
      clearInterval(this.timer);
    }
  },
};
</script>

<style scoped>
@font-face {
  font-family: "digital-7";
  src: url("../assets/fonts/digital-7.ttf") format("truetype");
}
.datebox {
  font-size: 30px;
  color: #b4e4ff;
  font-family: "digital-7";
  width: 90%;
  height: auto;
  text-align: center;
  position: absolute;
  bottom: 0;
}
</style>
