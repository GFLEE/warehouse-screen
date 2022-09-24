<template>
  <datav />
</template>

<script>
import datav from "./Index.vue";

export default {
  name: "Screen",
  components: {
    datav,
  },
  props: {},
  data() {
    return {
      fjk_data: "",
      spinning: false,
      zjk_data: "",
    };
  },

  mounted() {},
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped></style>
