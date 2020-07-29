<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="咨询类型" class="flex-100" prop="names">
          <el-input v-model="dataInfo.serviceTypeValue" class="yh-width-fixed" :disabled="true"></el-input>
      </el-form-item>
      <el-form-item label="价格" class="flex-100" prop="serviceExpense">
          <el-input placeholder="请输入价格" v-model="dataInfo.serviceExpense" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="次数" class="flex-100" prop="serviceFrequency">
          <el-input placeholder="请输入次数" v-model="dataInfo.serviceFrequency" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item v-if="isType('Phone')" label="咨询时长" class="flex-100" prop="serviceDuration">
          <el-input placeholder="咨询时长/分钟" v-model="dataInfo.serviceDuration" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="服务状态" class="flex-100">
        <template>
          <el-radio-group v-model="dataInfo.serviceState">
            <el-radio :label="true">开启</el-radio>
            <el-radio :label="false">关闭</el-radio>
          </el-radio-group>
        </template>
      </el-form-item>
      
    </el-form>
    <div class="form-footer">
        <el-button type="primary" @click="update()"
           >确定</el-button>
    </div>           
</section>
</template>
<script>
import axios from "axios";
import va from "@/rules";
import formMixns from "@/mixins/form";
export default {
  mixins: [formMixns], 
  data() {
    let required = va.required();
    let number = va.number();
    let double=va.double();
    let oneup=va.oneup();
    return {
      dataInfo:{
        serviceState:true,
      },
      rules: {//校验规则
        serviceExpense: [required,oneup],
        serviceFrequency: [required,number],
        serviceDuration: [required,number]
      }
    };
  },
  methods: {
    update() {
      this.updateDataInfo(this.$api.consultsetUpdate,this.dataInfo,"list");
    }
  },
  created(){
    this.getDataInfo(this.$api.consultsetData,{id:this.$route.query.id});
  }
};
</script>

<style scoped>
</style>