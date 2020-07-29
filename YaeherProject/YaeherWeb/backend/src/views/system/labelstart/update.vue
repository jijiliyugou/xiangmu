<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="标签类型" class="flex-100" prop="labelTypeCode">
          <el-select placeholder="请选择类型" v-model="dataInfo.labelTypeCode" @change="getName" :disabled="true">
              <el-option v-for="(item,key) in codeType" :key="key" :value="item.code" :label="item.name"></el-option>
          </el-select>
      </el-form-item>
      <el-form-item label="标签名称" class="flex-100" prop="labelName">
          <el-input placeholder="请输入标签名称" v-model="dataInfo.labelName" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="标签编号" class="flex-100" prop="labelCode">
          <el-input placeholder="请输入标签编号" v-model="dataInfo.labelCode" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="排序序号" class="flex-100" prop="orderSort">
          <el-input placeholder="请输入排序序号" v-model="dataInfo.orderSort" class="yh-width-fixed"></el-input>
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
    let chinese=va.checkChinese();
    return {
      rules: {//校验规则
        labelTypeCode: [required],
        orderSort: [required,number],
        labelName: [required],
        labelCode: [required,chinese]
      },
      codeType:[],
      paramStatus:{
        type:'ConfigPar',
        systemCode:'LabelManage'
      }
    };
  },
  methods: {
    update() {
      this.updateDataInfo(this.$api.labelstartUpdate,this.dataInfo,"list");
    },
    getName(val){
      const obj=this.codeType;
      for(var i=0;i<obj.length;i++){
        if(obj[i].code==val){
           this.dataInfo.labelTypeName=obj[i].name;
        }
      };
    }
  },
  created(){
    axios
        .all([
          axios.post(this.$api.baseParams,this.paramStatus), //参数获取 
          axios.post(this.$api.labelstartData,{id:this.id}), //默认数据 
        ])
        .then(
          axios.spread((base,data) => {
            this.codeType=base.result.item;
            this.dataInfo=data.result.item;
            this.loading = false;
          })
        )
        .catch(err => {
          this.errorMessage('获取数据失败');
          this.loading = false;
        });
  }
};
</script>

<style scoped>
</style>