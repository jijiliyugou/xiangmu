<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="端口名称" class="flex-100" prop="interfaceName">
          <el-input v-model="dataInfo.interfaceName" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="端口说明" class="flex-100" prop="interfaceIntro">
          <el-input v-model="dataInfo.interfaceIntro" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="端口地址" class="flex-100" prop="interfaceAddress">
          <el-input v-model="dataInfo.interfaceAddress" class="yh-width-fixed"></el-input>
      </el-form-item>
    <div class="form-footer">
        <el-button :loading="loading" type="primary" v-if="isType('add')" @click="update()"
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
    return {
      rules: {//校验规则
        interfaceName: [required],
        interfaceIntro: [required],
        interfaceAddress: [required]
      }
    };
  },
  methods: {
    update() {
      this.saveDataInfo(this.$api.portAdd,this.dataInfo,"list");
    }
  },
  created(){
    this.loading=false;
  }
};
</script>

<style scoped>
</style>