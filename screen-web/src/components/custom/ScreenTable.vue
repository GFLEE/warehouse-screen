<template>
  <div>
    <div class="table-head">
      <table class="layui-table" style="border-collapse: collapse">
        <colgroup>
          <col
            v-for="(colItem, idx) in cols"
            :key="'col-' + idx"
            :style="{ width: getColWidth(colItem) }"
          />
        </colgroup>
        <thead>
          <tr>
            <th
              v-for="(colItem, idx2) in cols"
              class="st-header"
              :key="'th-' + idx2"
              :style="{ textAlign: getAlign(colItem) }"
            >
              {{ colItem.title }}
            </th>
          </tr>
        </thead>
      </table>
    </div>

    <div class="table-body" :style="{ height: getHeight() }">
      <table class="layui-table" style="border-collapse: collapse">
        <colgroup>
          <col
            v-for="(colItem, idx3) in cols"
            :key="'col2-' + idx3"
            :style="{ width: getColWidth(colItem) }"
          />
        </colgroup>
        <tbody v-if="data && data.length > 0">
          <tr
            v-for="(dtItem, idx5) in data"
            :key="'dt-' + idx5"
            :class="[idx5 % 2 == 1 ? 'color_ji' : 'color_ou']"
          >
            <td
              v-for="(colItem, idx6) in cols"
              :key="'col3-' + idx6"
              :style="{
                textAlign: getAlign(colItem),
                color: getFontColor(colItem, dtItem),
              }"
            >
              <div v-if="colItem.type && colItem.type == 'num'">
                {{ idx5 + 1 }}
              </div>
              <div v-else>{{ dtItem[colItem.field] }}</div>
            </td>
          </tr>
        </tbody>
        <tbody v-else></tbody>
      </table>
    </div>
  </div>
</template>

<script>
export default {
  name: "ScreenTable",
  props: {
    cols: {
      type: Array,
      default() {
        return [
          { type: "num", title: "序号" },
          { field: "field1", title: "字段1" },
          { type: "field2", title: "字段2" },
        ];
      },
    },
    data: {
      type: Array,
      default() {
        return [];
      },
    },
    isload: {
      type: Boolean,
      default() {
        return false;
      },
    },
    height: {
      type: Number,
      default() {
        return 190;
      },
    },
  },
  data() {
    return {
      datas: [],
    };
  },
  methods: {
    loadMore() {
      return false;
    },
    getTitle(colItem) {
      const title = colItem.title;
      if (title) {
        return title;
      } else {
        if (colItem.type && colItem.type == "num") {
          return "序号";
        } else {
          return colItem.field;
        }
      }
    },
    getColWidth(colItem) {
      const type = colItem.type;
      const width = colItem.width;
      if (width) {
        if ((width + "").slice(-1) == "%" || (width + "").slice(-2) == "px") {
          return width;
        } else {
          return width + "px";
        }
      } else {
        if (type && "num" == type) {
          return 80 + "px";
        } else {
          return "auto";
        }
      }
    },
    getAlign(colItem) {
      const align = colItem.align;
      if (align && ("center" == align || "left" == align || "right" == align)) {
        return align;
      } else {
        return "left";
      }
    },
    getFontColor(colItem, dtItem) {
      const fontColor = colItem.fontColor;
      if (fontColor == null || fontColor == undefined) {
        return "#ffffff";
      } else {
        var value = dtItem[colItem.field];
        for (var key in fontColor) {
          if (key == value) {
            return fontColor[key];
          }
        }
        return fontColor;
      }
    },
    getHeight() {
      if (
        (this.height + "").slice(-1) == "%" ||
        (this.height + "").slice(-2) == "px"
      ) {
        return this.height;
      } else {
        if (Number(this.height) < 90) {
          return "90px";
        } else {
          return this.height + "px";
        }
      }
    },
  },
};
</script>

<style lang="less" scoped>
.table-body {
  width: 100%;
  overflow-y: auto;
}

.table-body::-webkit-scrollbar {
  width: 0;
}

table.layui-table {
  margin: 0;
  cellspacing:0;
  width: 100%;
  color: #ffffff;
//   background: rgba(14, 43, 117, 0.5);
  thead {
    tr {
        background:rgba(0, 44, 81, 0.8)
    //   background: rgba(14, 43, 117, 0.5);
    }
  }
  // tr {
  //   width: 100%;
  //   display: flex;
  //   overflow: hidden;
  //   background: rgba(14, 43, 117, 0.5);
  // }
  th {
    background: rgb(44, 62, 80);
  }
  td {
    padding: 0.8em;
    font-size:0.8em;
    border: none;
    font-weight:normal;

  }
  .color1 {
    background: #07164e;
  }
.color_ji{
  background:rgba(0, 44, 81, 0.8)

}
.color_ou{

}

.st-header{
    height:2em;
    font-size:0.9em;
    font-weight:normal;


}

}
</style>
