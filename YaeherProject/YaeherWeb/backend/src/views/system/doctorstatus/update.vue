<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
        <el-form-item label="医生名称" class="flex-100" prop="doctorName">
          <el-input v-model="dataInfo.doctorName" class="yh-width-fixed" :disabled="true"></el-input>
      </el-form-item>
      <el-form-item label="分成比例" class="flex-100" prop="divideInto">
          <el-input v-model="dataInfo.divideInto" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="回款天数" class="flex-100" prop="incomeDay">
          <el-input v-model="dataInfo.incomeDay" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="价格浮动值" class="flex-100" prop="doctorMoneyExchange">
          <el-input v-model="dataInfo.doctorMoneyExchange" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="限制天数" class="flex-100" prop="doctorMoneyexTime">
          <el-input v-model="dataInfo.doctorMoneyexTime" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="上下线状态" class="flex-100">
        <template>
          <el-radio-group v-model="dataInfo.onlineState">
            <el-radio :label="'online'">上线</el-radio>
            <el-radio :label="'offline'">下线</el-radio>
          </el-radio-group>
        </template>
      </el-form-item>
    <div class="form-footer">
        <el-button :loading="loading" type="primary" v-if="isType('update')" @click="update()"
           >确定</el-button>
    </div>
  </el-form>           
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
    let double = va.double();
    return {
      rules: {//校验规则
        divideInto: [required,double],
        incomeDay: [required,number],
        doctorMoneyExchange:[required,double],
        doctorMoneyexTime:[required,number]
      }
    };
  },
  methods: {
    update() {
      this.updateDataInfo(this.$api.doctorstatusUpdate,this.dataInfo,"list");
    }
  },
  created(){  



    this.getDataInfo(this.$api.doctorstatusData,{id:this.$route.query.id});
  }
};
</script>

<style scoped>
</style>