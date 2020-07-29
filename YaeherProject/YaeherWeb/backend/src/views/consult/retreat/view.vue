<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back> 
    <el-form  ref="form" :model="dataInfo"
        class="yh-form" label-width="150px">
      <el-form-item label="退单编号：" class="flex-100">
          <span class="itemval" v-text="dataInfo.refundNumber"></span>
      </el-form-item>
      <el-form-item label="订单编号：" class="flex-100">
          <span class="itemval" v-text="dataInfo.orderNumber"></span>
      </el-form-item>
      <el-form-item label="咨询单号：" class="flex-100">
          <span class="itemval" v-text="dataInfo.consultNumber"></span>
      </el-form-item>
      <el-form-item label="咨询姓名：" class="flex-100">
          <span class="itemval" v-text="dataInfo.consultantName"></span>
      </el-form-item>
      <el-form-item label="患者姓名：" class="flex-100">
          <span class="itemval" v-text="dataInfo.patientName"></span>
      </el-form-item>
      <el-form-item label="医生姓名：" class="flex-100">
          <span class="itemval" v-text="dataInfo.doctorName"></span>
      </el-form-item>
      <el-form-item label="退单金额：" class="flex-100">
          <span class="itemval" v-text="dataInfo.orderMoney+' '+dataInfo.orderCurrency"></span>
      </el-form-item>
      <el-form-item label="问题描述：" class="flex-100">
          <span class="itemval" v-text="dataInfo.iiInessDescription"></span>
      </el-form-item>
      <el-form-item label="退单时间：" class="flex-100">
          <span class="itemval" v-text="formatTime(dataInfo.createdOn)"></span>
      </el-form-item>
      <el-form-item label="退单类型：" class="flex-100">
          <span class="itemval" v-text="dataInfo.refundType"></span>
      </el-form-item>
      <!-- <el-form-item label="退单状态：" class="flex-100">
          <span class="itemval" v-text="dataInfo.refundState"></span>
      </el-form-item> -->
      <!-- <el-form-item label="退单原因：" class="flex-100">
          <span class="itemval" v-text="JSON.parse(dataInfo.refundReason).LabelName"></span>
      </el-form-item> -->
      <el-form-item label="退单理由：" class="flex-100">
          <span class="itemval" v-text="dataInfo.refundRemarks"></span>
      </el-form-item>
    </el-form>       
    <div class="form-footer">
        <el-button type="success"  @click="$router.go(-1)"
           >返回</el-button>
    </div>           
</section>
</template>
<script>
import formMixns from "@/mixins/form";
export default {
  mixins: [formMixns],
  data() {
    return {
      dataInfo:{
      }
    }
  },
  created(){
    this.getDataInfo(this.$api.retreatDetail,{refundNumber:this.$route.query.id});
  }
};
</script>

<style scoped>
.yh-form .el-form-item{padding:0px;}
.itemval{display: inline-block;max-width: 500px;line-height: 20px;margin-top: 6px;word-break: break-all;}
</style>