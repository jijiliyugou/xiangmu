<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="角色名称" class="flex-100" prop="roleName">
          <el-input v-model="dataInfo.roleName" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="RoleCode" class="flex-100" prop="roleCode">
          <el-input v-model="dataInfo.roleCode" class="yh-width-fixed" disabled="true"></el-input>
      </el-form-item>
      <el-form-item label="备注" class="flex-100" prop="description">
          <el-input v-model="dataInfo.description" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="是否激活" class="flex-100">
        <template>
          <el-radio-group v-model="dataInfo.enabled">
            <el-radio :label="true">是</el-radio>
            <el-radio :label="false">否</el-radio>
          </el-radio-group>
        </template>
      </el-form-item>
      <el-form-item label="是否管理员" class="flex-100">
        <template>
          <el-radio-group v-model="dataInfo.isAdmin">
            <el-radio :label="true">是</el-radio>
            <el-radio :label="false">否</el-radio>
          </el-radio-group>
        </template>
      </el-form-item>
    </el-form>
    <div class="form-footer">
        <el-button type="primary" v-if="isType('update')" @click="update()"
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
        roleName: [required],
        description: [required]
      }
    }
  },
  methods: {
    update() {
      this.updateDataInfo(this.$api.roleUpdate,this.dataInfo,"list");
    }
  },
  created(){
    this.getDataInfo(this.$api.roleData,{id:this.$route.query.id});
  }
};
</script>

<style scoped>
</style>