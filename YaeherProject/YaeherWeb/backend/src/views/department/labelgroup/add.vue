<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="组名称" class="flex-100" prop="groupName">
          <el-input v-model="dataInfo.groupName" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="标签" class="flex-100" prop="lableID">
          <el-checkbox-group v-model="checkedRole" @change="handleCheckedRolesChange">
              <el-checkbox v-for="role in roles" :label="role.id" :key="role.id">{{role.lableName}}</el-checkbox>
          </el-checkbox-group>
      </el-form-item>
    </el-form>
    <div class="form-footer">
        <el-button :loading="loading" type="primary" v-if="isType('add')" @click="update()"
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
    return {
      checkedRole:[],
      roles:[],
      checkedName:[],
      rules: {//校验规则
        groupName: [required],
        lableID: [required]
      }
    };
  },
  methods: {
    update() {
      this.dataInfo.lableID=this.checkedRole.join(",");
      this.dataInfo.lableName=this.checkedName.join(",");
      this.saveDataInfo(this.$api.labelgroupAdd,this.dataInfo,"list");
    },
    handleCheckedRolesChange(value) {
      this.checkedRole=value;
      this.checkedName=[];
      for(var i=0;i<value.length;i++){
        for(var j=0;j<this.roles.length;j++){
          if(value[i]==this.roles[j].id){
            this.checkedName.push(this.roles[j].lableName);
          }
        }
      }
      //let checkedCount = value.length;
      //this.checkAll = checkedCount === this.roles.length;
      //this.isIndeterminate = checkedCount > 0 && checkedCount < this.roles.length;
    }
  },
  created(){
    this.$http.post(this.$api.labelListnopage).then(res=>{
      this.roles=res.result.item;
      this.loading=false;
    }).catch(error=>{this.errorMessage("获取数据失败");});
  }
};
</script>

<style scoped>
</style>