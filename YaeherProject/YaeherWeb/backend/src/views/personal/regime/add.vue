<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="规则类型" class="flex-100" prop="rulesType">
          <el-select placeholder="请选择规则类型" v-model="dataInfo.rulesType">
               <el-option v-for="(item,key) in ruleType" :key="key" :value="item.itemValue" :label="item.remark"></el-option>
          </el-select>
      </el-form-item>
      <el-form-item label="规则标题" class="flex-100" prop="rulesTitle">
          <el-input v-model="dataInfo.rulesTitle" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="适用科室" class="flex-100" prop="applyClinicID">
        <el-checkbox-group v-model="checkedRole" @change="handleCheckedRolesChange">
            <el-checkbox v-for="role in roles" :label="role.id" :key="role.id">{{role.clinicName+(role.clinicType==1?'(成人)':'(儿童)')}}</el-checkbox>
        </el-checkbox-group>
      </el-form-item>
      <el-form-item label="规则内容" class="flex-100 yeditor" prop="rulesContent">
        <div class="editorbox">
          <yh-editor :defaultMsg="defaultContent" :index="0" @ready="getUeditor"></yh-editor>
        </div>
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
    let number = va.number();
    return {
      defaultContent:'',
      ruleType:[],
      roles:[],
      checkedRole:[],
      paramStatus:{
        type:'ConfigPar',
        systemCode:'DoctorRulesType'
      },
      rules: {//校验规则
        rulesType:[required],
        rulesTitle: [required],
        rulesContent: [required],
        applyClinicID:[required]
      },
      ue:[]
    };
  },
  methods: {
    update() {
      this.dataInfo.CheckState='created';
      this.dataInfo.applyClinicID=this.checkedRole.join(",");
      this.dataInfo.rulesContent=this.ue[0].getContent();
      this.saveDataInfo(this.$api.regimeAdd,this.dataInfo,"list");
    },
    handleCheckedRolesChange(value) {
      this.checkedRole=value;
    },
    getUeditor(ue,index){
      this.ue[index]=ue;//获取ue实例 
    }
  },
  created(){
    axios
        .all([
          axios.post(this.$api.baseParams,this.paramStatus), //规则类型
          axios.post(this.$api.partListnopage), //科室
        ])
        .then(
          axios.spread((rules,part) => {
            this.ruleType=rules.result.item;//退单理由
            this.roles=part.result.item;
            this.loading=false;
          })
        )
        .catch(err => {
          this.errorMessage('获取参数失败');
          this.loading = false;
        });
  }
};
</script>

<style scoped>
</style>