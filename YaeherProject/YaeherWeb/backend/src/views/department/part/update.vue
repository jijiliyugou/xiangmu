<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="科室名称" class="flex-100" prop="clinicName">
          <el-input v-model="dataInfo.clinicName" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="科室类型" class="flex-100" prop="clinicType">
          <template>
            <el-radio-group v-model="dataInfo.clinicType">
              <el-radio :label="1">成人</el-radio>
              <el-radio :label="2">儿童</el-radio>
            </el-radio-group>
          </template>
      </el-form-item>
      <el-form-item label="排序序号" class="flex-100" prop="orderSort">
          <el-input placeholder="请输入排序序号" v-model="dataInfo.orderSort" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="科室介绍" class="flex-100" prop="clinicIntro">
          <el-input type="textarea" :rows="6" v-model="dataInfo.clinicIntro" class="yh-width-fixed"></el-input>
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
        clinicName: [required],
        clinicType: [required],
        orderSort: [required,number],
        clinicIntro: [required]
      }
    };
  },
  methods: {
    update() {
      this.updateDataInfo(this.$api.partUpdate,this.dataInfo,"list");
    }
  },
  created(){
    this.getDataInfo(this.$api.partData,{id:this.id});
  }
};
</script>

<style scoped>
</style>