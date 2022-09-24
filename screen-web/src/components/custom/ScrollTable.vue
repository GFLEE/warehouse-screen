<template>
  <div
    class="table-wrapper"
    :style="{
      width:
        'calc(' +
        widths.reduce((i, j) => `${i} + ${j}`) +
        ` + ${(widths.length - 1) * 6}px)`,
      height: `calc(${contentHeight} + 48px)`,
    }"
  >
    <div class="table-header" v-if="showHeader">
      <span v-for="(it, i) in widths" :key="i" :style="{ width: widths[i] }">{{
        titles[i]
      }}</span>
    </div>
    <div class="table-content" :style="{ height: contentHeight }">
      <swiper class="swiper" :options="swiperOption" ref="myBotSwiper">
        <swiper-slide v-for="(it, i) in data" :key="i">
          <div
            class="table-row"
            :class="{ stripe: i % 2 === 1, active: i === currentActiveRow }"
            @click="handleRowClick(it, i)"
            :style="{ height: tabelHeight }"
          >
            <span
              v-for="(ite, ind) in widths"
              :key="ind"
              :title="it[ind]"
              :style="{ width: widths[ind], height: tabelHeight }"
              >{{ it[ind] }}</span
            >
          </div>
        </swiper-slide>
      </swiper>
    </div>
  </div>
</template>

<script>
import "swiper/swiper-bundle.css";
// import { swiper, swiperSlide } from "vue-awesome-swiper";
import { swiper, swiperSlide } from 'vue-awesome-swiper'

export default {
  name: "swiper-table",
  components: {
    swiper,
    swiperSlide,
  },
  props: {
    // 传入的表格数据
    data: {
      type: Array,
      default: () => [
        [
          "杂草乱扔",
          "2020.08.23 12:26:15",
          "全要素网格化",
          "公安交警",
          "环境保护",
          "待受理",
          "李念省",
        ],
        [
          "杂草乱扔",
          "2020.08.23 12:26:15",
          "全要素网格化",
          "公安交警",
          "环境保护",
          "待受理",
          "李念省",
        ],
        [
          "杂草乱扔",
          "2020.08.23 12:26:15",
          "全要素网格化",
          "公安交警",
          "环境保护",
          "待受理",
          "李念省",
        ],
        [
          "杂草乱扔",
          "2020.08.23 12:26:15",
          "全要素网格化",
          "公安交警",
          "环境保护",
          "待受理",
          "李念省",
        ],
        [
          "杂草乱扔",
          "2020.08.23 12:26:15",
          "全要素网格化",
          "公安交警",
          "环境保护",
          "待受理",
          "李念省",
        ],
        [
          "杂草乱扔",
          "2020.08.23 12:26:15",
          "全要素网格化",
          "公安交警",
          "环境保护",
          "待受理",
          "李念省",
        ],
        [
          "杂草乱扔",
          "2020.08.23 12:26:15",
          "全要素网格化",
          "公安交警",
          "环境保护",
          "待受理",
          "李念省",
        ],
        [
          "杂草乱扔",
          "2020.08.23 12:26:15",
          "全要素网格化",
          "公安交警",
          "环境保护",
          "待受理",
          "李念省",
        ],
        [
          "杂草乱扔",
          "2020.08.23 12:26:15",
          "全要素网格化",
          "公安交警",
          "环境保护",
          "待受理",
          "李念省",
        ],
        [
          "杂草乱扔",
          "2020.08.23 12:26:15",
          "全要素网格化",
          "公安交警",
          "环境保护",
          "待受理",
          "李念省",
        ],
        [
          "杂草乱扔",
          "2020.08.23 12:26:15",
          "全要素网格化",
          "公安交警",
          "环境保护",
          "待受理",
          "李念省",
        ],
        [
          "杂草乱扔",
          "2020.08.23 12:26:15",
          "全要素网格化",
          "公安交警",
          "环境保护",
          "待受理",
          "李念省",
        ],
        [
          "杂草乱扔",
          "2020.08.23 12:26:15",
          "全要素网格化",
          "公安交警",
          "环境保护",
          "待受理",
          "李念省",
        ],
        [
          "杂草乱扔",
          "2020.08.23 12:26:15",
          "全要素网格化",
          "公安交警",
          "环境保护",
          "待受理",
          "李念省",
        ],
        [
          "杂草乱扔",
          "2020.08.23 12:26:15",
          "全要素网格化",
          "公安交警",
          "环境保护",
          "待受理",
          "李念省",
        ],
      ],
    },
    // 传入的表格头
    titles: {
      type: Array,
      default: () => [
        "事件标题",
        "创建时间",
        "信息来源",
        "分类",
        "状态",
        "上报人",
      ],
    },
    // 表格的列宽
    widths: {
      type: Array,
      default: () => ["60px", "60px", "60px", "60px", "60px", "60px"],
    },
    // 表格的高度
    contentHeight: {
      type: String,
      default: "36px",
    },
    // 是否展示表格头
    showHeader: {
      type: Boolean,
      default: true,
    },
    // 表格的行高
    tabelHeight: {
      type: String,
      default: "36px",
    },
  },
  computed: {
    myBotSwiper() {
      return this.$refs.myBotSwiper.$swiper;
    },
  },
  data() {
    return {
      currentActiveRow: 0,
      swiperOption: {
        autoHeight: true,
        direction: "vertical",
        spaceBetween: 0,
        autoplay: {
          delay: 2500,
          disableOnInteraction: false,
          autoplayDisableOnInteraction: false,
        },
        slidesPerView: "auto",
        grabCursor: true,
        autoplayDisableOnInteraction: false,
        mousewheelControl: true,
      },
    };
  },
  methods: {
    handleRowClick(it, i) {
      this.currentActiveRow = i;
      const currentIt = this.data[i];
      this.$emit("change", currentIt);
    },
    // 控制表格停止滚动
    swiperStop() {
      this.myBotSwiper.autoplay.stop();
    },
    // 控制表格开始滚动
    swiperStart() {
      this.myBotSwiper.autoplay.start();
    },
  },
  watch: {
    data() {
      this.currentActiveRow = 0;
    },
  },
};
</script>

<style lang="less" scoped>
.table-wrapper {
  width: 30%;
  height: auto;
  span {
    box-sizing: border-box;
  }
  .table-header {
    @height: 31px;
    width: 100%;
    height: @height;
    span {
      color: #83c2ee;
      //   background: rgba(255, 255, 255, 0.12);
      background-color: #013558;
      font-size: 19px;
      font-family: PingFangSC-Medium, PingFang SC;
      font-weight: 500;
      color: rgba(131, 194, 238, 1);
      line-height: @height;
      &:nth-child(2) {
        text-align: center;
      }
    }
    span + span {
      margin-left: 2px;
    }
  }
  .table-content {
    width: 100%;
    height: 284px;
    margin-top: 2px;
    .swiper {
      width: 100%;
      height: 100%;
      .table-row {
        width: 100%;
        height: 36px;
        cursor: pointer;
        &.active {
          span {
            color: rgba(131, 194, 238, 1);
            font-size: 17px;
            font-weight: bold;
          }
        }
        &.stripe span {
          background: rgba(255, 255, 255, 0.06);
        }
        span {
          font-size: 17px;
          font-family: PingFangSC-Regular, PingFang SC;
          font-weight: 400;
          color: rgba(255, 255, 255, 0.85);
          line-height: 36px;
        }
        span + span {
          margin-left: 2px;
        }
      }
    }
  }
  span {
    height: 100%;
    line-height: 100%;
    display: inline-block;
    text-align: center;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    margin-bottom: 6px;
    &:nth-child(1) {
      width: 246px;
    }
    &:nth-child(2) {
      width: 348px;
      padding-left: 13px;
    }
    &:nth-child(3) {
      width: 214px;
    }
    &:nth-child(4) {
      width: 214px;
    }
    &:nth-child(5) {
      width: 214px;
    }
  }
}
</style>
