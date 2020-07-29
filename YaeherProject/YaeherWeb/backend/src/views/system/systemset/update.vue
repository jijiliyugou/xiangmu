<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="系统名称" class="flex-100" prop="systemName">
          <el-input v-model="dataInfo.systemName" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="系统类型" class="flex-100" prop="systemType">
          <el-input v-model="dataInfo.systemType" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="支付秘钥" class="flex-100" prop="tenPayKey">
          <el-input v-model="dataInfo.tenPayKey" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="支付ID" class="flex-100" prop="tenPayKey">
          <el-input v-model="dataInfo.tenPayMchId" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="tenPayNotify" class="flex-100" prop="tenPayNotify">
          <el-input v-model="dataInfo.tenPayNotify" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="tenPayRefundNotify" class="flex-100" prop="tenPayRefundNotify">
          <el-input v-model="dataInfo.tenPayRefundNotify" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="accessKeyID" class="flex-100" prop="accessKeyID">
          <el-input v-model="dataInfo.accessKeyID" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="accessSecret" class="flex-100" prop="accessSecret">
          <el-input v-model="dataInfo.accessSecret" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="appID" class="flex-100" prop="appID">
          <el-input v-model="dataInfo.appID" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="appKey" class="flex-100" prop="appKey">
          <el-input v-model="dataInfo.appKey" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="appSecret" class="flex-100" prop="appSecret">
          <el-input v-model="dataInfo.appSecret" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="callCenterNumber" class="flex-100" prop="callCenterNumber">
          <el-input v-model="dataInfo.callCenterNumber" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="refreshTokenCode" class="flex-100" prop="refreshTokenCode">
          <el-input v-model="dataInfo.refreshTokenCode" class="yh-width-fixed"></el-input>
      </el-form-item>
    </el-form>
    <div class="form-footer">
        <el-button :loading="loading" type="primary" v-if="isType('update')" @click="update()"
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
    return {
      rules: {//校验规则
        systemName: [required],
        systemType: [required],
        tenPayKey: [required],
        tenPayMchId: [required]
      }
    };
  },
  methods: {
    update() {
      this.updateDataInfo(this.$api.systemSetUpdate,this.dataInfo,"list");
    }
  },
  created(){
    this.getDataInfo(this.$api.systemSetData,{id:this.$route.query.id});
  }
};
</script>

<style scoped>
</style>