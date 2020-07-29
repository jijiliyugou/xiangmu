<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <!-- <el-form-item label="理由" class="flex-100" prop="code">
        <el-select placeholder="请选择退单理由" v-model="dataInfo.code">
             <el-option v-for="(item,key) in resonType" :key="key" :value="item.labelCode" :label="item.labelName"></el-option>
        </el-select>
      </el-form-item> -->
      <el-form-item label="推荐医生" class="flex-100" prop="parentId">
        <el-cascader
          :options="options"
          v-model="selectedOptions">
        </el-cascader>
      </el-form-item>
      <el-form-item label="退单原因描述" class="flex-100" prop="reson">
        <div class="reasonbox"> 
          <el-input type="textarea" :rows="6" placeholder="请描述退单的具体原因" v-model="dataInfo.reson" class="yh-width-fixed" @keyup.native="keyon" @change="key"></el-input>
          <label class="lettertype">{{letterNum}}/{{maxLength}}</label>
        </div>
      </el-form-item>
    </el-form>
    <div class="form-footer">
        <el-button type="primary" v-if="isType('reback')" @click="update()"
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
      maxLength:300,
      letterNum:0,
      resonType:[],
      rules: {//校验规则
        reson: [required]
      },
      options: [],
      selectedOptions: []
    };
  },
  methods: {
    keyon(){
      if(this.dataInfo.reson!=undefined){
        this.letterNum=this.dataInfo.reson.length;
        if(this.letterNum>this.maxLength){
          this.dataInfo.reson=this.dataInfo.reson.substring(0,this.maxLength);
          this.letterNum=this.dataInfo.reson.length;
        }
      }
    },
    key(){
      if(this.dataInfo.reson!=undefined){
        this.letterNum=this.dataInfo.reson.length;
        if(this.letterNum>this.maxLength){
          this.dataInfo.reson=this.dataInfo.reson.substring(0,this.maxLength);
          this.letterNum=this.dataInfo.reson.length;
        }
      }
    },
    update() {
      this.dataInfo.doctorId=this.selectedOptions[1];
      this.dataInfo.reson=this.dataInfo.reson.replace(/\n|\r/g,'<br/>');
      this.handleDataInfo(this.$api.referBack,{refundReason:'',recommendDoctorID:this.selectedOptions[1],refundRemarks:this.dataInfo.reson,consultID:this.$route.query.id},"list");
    }/*,
    handleChange(value) {
      console.log(value);
      console.log(this.selectedOptions[1]);
    }*/
  },
  created(){
    axios
        .all([
          //axios.post(this.$api.labelRelative,{LabelTypeCode:'DoctorReturnLabel'}), //退单理由
          axios.post(this.$api.referPartdoctor), //推荐医生 
          axios.post(this.$api.getType), //基础参数获取
        ])
        .then(
          axios.spread((doctor,base) => {
            //this.resonType=reson.result.item[0].children;//退单理由
            const arr=doctor.result.item;
            this.maxLength=base.result.item.maxReplyLength;
            const newArr=[];
            for(let i=0;i<=arr.length-1;i++){
               const obj={};
               obj.label=arr[i].clinicInfomation.clinicName;
               obj.value=arr[i].clinicInfomation.id;
               if(arr[i].clinicDoctorReltion.length>0){
                 obj.children=[];
                 for(let j=0;j<=arr[i].clinicDoctorReltion.length-1;j++){
                   const childA={};
                   childA.label=arr[i].clinicDoctorReltion[j].doctorName;
                   childA.value=arr[i].clinicDoctorReltion[j].doctorID;
                   obj.children.push(childA);
                 }
               }
               newArr.push(obj);
            }
            this.options=newArr; 
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