<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back> 
    <el-form  ref="form" :model="dataInfo"
        class="yh-form" label-width="150px">
      <el-form-item label="订单编号：" class="flex-100">
          <span class="itemval" v-text="dataInfo.orderNumber"></span>
      </el-form-item>
      <el-form-item label="咨询单号：" class="flex-100">
          <span class="itemval" v-text="dataInfo.consultNumber"></span>
      </el-form-item>
      <el-form-item label="支付方式：" class="flex-100">
          <span class="itemval" v-text="dataInfo.payType"></span>
      </el-form-item>
      <el-form-item label="支付币别：" class="flex-100">
          <span class="itemval" v-text="dataInfo.orderCurrency"></span>
      </el-form-item>
      <el-form-item label="支付账号：" class="flex-100">
          <span class="itemval" v-text="dataInfo.tenpayNumber"></span>
      </el-form-item>
      <el-form-item label="支付金额：" class="flex-100">
          <span class="itemval" v-text="dataInfo.payMoney"></span>
      </el-form-item>
      <el-form-item label="支付状态：" class="flex-100">
          <span class="itemval" v-text="dataInfo.orderState"></span>
      </el-form-item>
      <el-form-item label="返回流水号：" class="flex-100">
          <span class="itemval" v-text="dataInfo.paySerialNumber"></span>
      </el-form-item>
      <el-form-item label="新增时间：" class="flex-100">
          <span class="itemval" v-text="formatTime(dataInfo.createdOn)"></span>
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
    this.getDataInfo(this.$api.payrecordDetail,{id:this.$route.query.id});
  }
};
</script>

<style scoped>
.yh-form .el-form-item{padding:0px;}
.itemval{display: inline-block;max-width: 500px;line-height: 20px;margin-top: 6px;}
</style>