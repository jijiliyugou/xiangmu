<template>
  <section class="yh-body" v-loading="loading">
    <!-- 返回组件 -->
    <yh-back></yh-back>        
    <el-form  ref="form" :model="dataInfo" :rules="rules" 
        class="yh-form" label-width="162px">
      <el-form-item label="用户角色" class="flex-100" prop="roleCode">
          <el-select placeholder="请选择用户角色" v-model="dataInfo.roleCode" @change="getName" :disabled="this.id!=0">
              <el-option v-for="(item,key) in codeType" :key="key" :value="item.code" :label="item.name"></el-option>
          </el-select>
      </el-form-item>
      <el-form-item label="自定义名称" class="flex-100" prop="conditionalName">
          <el-input placeholder="请输入自定义名称" v-model="dataInfo.conditionalName" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="自定义类型" class="flex-100" prop="conditionalType">
          <el-input placeholder="请输入自定义类型" v-model="dataInfo.conditionalType" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="自定义类型名称" class="flex-100" prop="conditionalTypeName">
          <el-input placeholder="请输入自定义类型名称" v-model="dataInfo.conditionalTypeName" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="自定义路径" class="flex-100" prop="conditionalUrl">
          <el-input placeholder="请输入自定义路径" v-model="dataInfo.conditionalUrl" class="yh-width-fixed"></el-input>
      </el-form-item>
      <el-form-item label="页面路径" class="flex-100" prop="pagepath">
          <el-input placeholder="请输入页面路径" v-model="dataInfo.pagepath" class="yh-width-fixed"></el-input>
      </el-form-item>
    </el-form>
    <div class="form-footer">
        <el-button type="primary" @click="update()"
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
      dataInfo:{
        parentID:0,
        appID:'',
        menuID:0
      },
      rules: {//校验规则
        roleCode: [required],
        conditionalName: [required],
        conditionalType: [required],
        conditionalTypeName: [required],
        conditionalUrl: [required,chinese],
        pagepath: [required]
      },
      codeType:[],
      paramStatus:{
        type:'ConfigPar',
        systemCode:'WXRole'
      }
    };
  },
  methods: {
    update() {
      this.dataInfo.conditionalUrl=this.dataInfo.conditionalUrl.trim();
      this.dataInfo.pagepath=this.dataInfo.pagepath.trim();
      this.saveDataInfo(this.$api.wechatAdd,this.dataInfo,"list");
    },
    getName(val){
      const obj=this.codeType;
      for(var i=0;i<obj.length;i++){
        if(obj[i].code==val){
           this.dataInfo.roleName=obj[i].name;
        }
      };
    }
  },
  created(){
    axios
        .all([
          axios.post(this.$api.baseParams,this.paramStatus), //参数获取
        ])
        .then(
          axios.spread((data) => {
            this.codeType=data.result.item;
            this.dataInfo.parentID=this.id;
            if(this.type!='add'){
              this.dataInfo.roleCode=this.type.split(",")[0];
              this.dataInfo.roleName=this.type.split(",")[1];
            }
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