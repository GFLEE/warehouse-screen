<template>
  <div style="background-color: #ececec; padding: 20px; height: auto">
    <a-row>
      <a-col :span="12">
        <a-card title="fjk_data" :bordered="false" style="height: auto">
          <div id="codeView" v-highlight>
            <pre><code class="json"  v-html="fjk_data"></code></pre>
          </div>
        </a-card>
      </a-col>
      <a-col :span="12">
        <a-card title="zjk_data" :bordered="false" style="height: auto">
          <div id="codeView" v-highlight>
            <pre><code class="json"  v-html="zjk_data"></code></pre>
          </div>
        </a-card>
      </a-col>
    </a-row>
  </div>
</template>

<script>
import { connectServer } from "@/utils/signalR";
import bus from "@/utils/bus";

export default {
  name: "HelloWorld",
  props: {
    msg: String,
  },
  data() {
    return {
      fjk_data: "",
      spinning: false,
      zjk_data: "",
    }
  },

  mounted() {
    const v_url = "http://127.0.0.1:5020/InterfaceServiceHub";
    connectServer(v_url, "virtual data");

    bus.$on("fjk_data", (data) => {
      this.fjk_data = "";
      this.fjk_data = data;
    });

    bus.$on("zjk_data", (data) => {
      this.zjk_data = "";
      this.zjk_data = data;
    });
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
 
</style>
