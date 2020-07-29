<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="医生名称" class="flex-100" prop="doctorName">
          <el-input v-model="dataInfo.doctorName" class="yh-width-fixed" :disabled="true"></el-input>
      </el-form-item>
      <el-form-item label="排班时间" class="flex-100" prop="schedulingDate">
          <el-date-picker
            v-model="dataInfo.schedulingDate"
            type="date"
            value-format="yyyy-MM-dd"
            placeholder="选择日期">
          </el-date-picker>
      </el-form-item>
      <el-form-item label="排班时段" class="flex-100" prop="schedulingTimeList">
          <el-checkbox-group v-model="checkedTime" @change="handleCheckedRolesChange">
              <el-checkbox v-for="role in schedulingType" :label="role.code" :key="role.code">{{role.value}}</el-checkbox>
          </el-checkbox-group>
      </el-form-item>
      <el-form-item label="重复方式" class="flex-100" prop="duplication">
          <el-select placeholder="请选择重复方式" v-model="dataInfo.duplication">
             <el-option v-for="(item,key) in duplication" :key="key" :value="item.code" :label="item.value"></el-option>
          </el-select>
      </el-form-item>
      <el-form-item label="门诊类型" class="flex-100" prop="clinicType">
          <el-select placeholder="请选择门诊类型" v-model="dataInfo.clinicType">
             <el-option v-for="(item,key) in clinicType" :key="key" :value="item.code" :label="item.value"></el-option>
          </el-select>
      </el-form-item>
      <el-form-item label="门诊地点" class="flex-100" prop="clinicIDAdd">
          <el-input v-model="dataInfo.clinicIDAdd" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="挂号费" class="flex-100" prop="registrationFee">
          <el-input v-model="dataInfo.registrationFee" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="是否开启" class="flex-100">
        <template>
          <el-radio-group v-model="dataInfo.serviceState">
            <el-radio :label="true">是</el-radio>
            <el-radio :label="false">否</el-radio>
          </el-radio-group>
        </template>
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
    let double = va.double();
    return {
      clinicType:[],
      duplication:[],
      schedulingType:[],
      roles:'',
      checkedTime:[],
      dataInfo:{
        serviceState:true
      },
      rules: {//校验规则
        doctorName: [required],
        schedulingDate: [required],
        schedulingTimeList: [required],
        duplication: [required],
        clinicType: [required],
        clinicIDAdd: [required],
        registrationFee: [required,double]
      }
    };
  },
  methods: {
    update() {
      const codeArr=this.checkedTime.map(function(data){
        return {code:data};
      });
      this.dataInfo.schedulingTimeList=codeArr;
      //this.dataInfo.clinicType={code:this.dataInfo.clinicType}.toString();
      //this.dataInfo.duplication={code:this.dataInfo.duplication}.toString();
      this.updateDataInfo(this.$api.arrangeUpdate,this.dataInfo,"list");
    },
    handleCheckedRolesChange(value) {
      //this.checkedTime=value;
    }
  },
  created(){
    this.loading=true;
    axios
        .all([
          axios.post(this.$api.clinicParams), //基础参数
          axios.post(this.$api.arrangeData,{id:this.$route.query.id}),//数据回填
        ])
        .then(
          axios.spread((base,data) => {
            this.loading=false;
            this.clinicType=base.result.item.clinicType;
            this.duplication=base.result.item.duplication;
            this.schedulingType=base.result.item.schedulingTime;
            this.dataInfo=data.result.item;
            this.dataInfo.clinicType=JSON.parse(this.dataInfo.clinicType).Code;
            this.dataInfo.duplication=JSON.parse(this.dataInfo.duplication).Code;
            this.checkedTime=JSON.parse(this.dataInfo.schedulingTime).map(function(data){return data.Code});
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