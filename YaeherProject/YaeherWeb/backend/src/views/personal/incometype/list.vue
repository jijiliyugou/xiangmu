<template>
  <section class="yh-body" v-loading="loading">      
    <el-form 
        class="yh-form" label-width="162px">
      <el-form-item label="收款方式" class="flex-100" prop="id">
          <template>
            <el-radio-group v-model="id">
              <el-radio v-for="(item,key) in typeList" :key="key" :label="item.id">{{item.payMethodName}}</el-radio>
              <el-radio :label="0" v-if="typeList.length==1">银行卡</el-radio>
            </el-radio-group>
          </template>
      </el-form-item>
      <el-form-item v-if="item.id==id" v-for="(item,key) in typeList" :key="key" :label="item.payMethod=='wxpay'?'微信账号：':'银行账号：'" class="flex-100">
          <span class="itemval" v-text="item.bankNo"></span><label class="bankupdate" v-if="item.payMethod=='bankcard'" @click="shageOpen('update')">修改</label>
      </el-form-item>
      <el-form-item v-if="typeList.length==1&&this.id==0" label="银行账号" class="flex-100">
          <span class="itemval" v-text="'暂无银行卡'"></span><label class="bankupdate" @click="shageOpen('add')">添加</label>
      </el-form-item>
    </el-form>
    <div class="form-footer">
        <el-button :loading="loading" type="primary" @click="update()"
           >保存</el-button>
    </div>
    <!-- 从业经历 -->
    <el-dialog
      class="shadebox"
      :title="shadeTitle"
      :visible.sync="bankDialogVisible"
      width="30%"
      height="400px"
      center>
      <el-form ref="form" :model="shadeForm" :rules="rules" label-width="100px">
        <el-form-item label="用户名">
            <span class="itemval" v-text="shadeForm.fullName"></span>
        </el-form-item>
        <el-form-item label="银行卡号" prop="bankNo">
          <el-input v-model="shadeForm.bankNo" placeholder="请输入银行卡号"></el-input>
        </el-form-item>
        <el-form-item label="银行名称" prop="bankName">
            <el-input v-model="shadeForm.bankName" placeholder="请输入银行名称"></el-input>
        </el-form-item>
        <el-form-item label="所属支行" prop="subbranch">
            <el-input v-model="shadeForm.subbranch" placeholder="请输入所属支行"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="bankDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="putIn()">确 定</el-button>
      </span>
    </el-dialog> 
    <!-- 申请质控 -->           
</section>
</template>
<script>
import axios from "axios";
import va from "@/rules";
export default {
  data() {
    let required = va.required();
    let number = va.number();
    return {
      loading:false,
      typeList:[],
      id:'',
      shadeTitle:'',
      bankDialogVisible:false,//修改银行卡
      shadeForm:{},
      dataFlag:true,
      rules: {//校验规则
        bankNo: [required,number],
        bankName: [required],
        subbranch: [required]
      },
    };
  },
  methods: {
    update() {
      if(this.id==0){
        this.errorMessage("请先添加银行卡");
        return false;
      }
      this.$http.post(this.$api.incomesetUpdate,{isDefault:1,id:this.id}).then(res=>{
        if(res.result.code=200){
          this.successMessage("设置成功");
        }
      }).catch(error=>{this.errorMessage("设置失败！")});
    },
    putIn(){//添加修改共用方法
      let form = this.$refs['form'];
      form.validate((valid) => {//验证方法
          if (valid) {
              if(this.dataFlag){
                this.$http.post(this.$api.bankcardAdd,this.shadeForm).then(res=>{
                  if(res.result.code=200){
                    this.successMessage("添加成功");
                    this.getData();
                    this.bankDialogVisible=false;
                  }
                }).catch(error=>{this.errorMessage("提交失败！")});
              }else{
                this.$http.post(this.$api.bankcardUpdate,this.shadeForm).then(res=>{
                  if(res.result.code=200){
                    this.successMessage("修改成功");
                    this.bankDialogVisible=false;
                  }
                }).catch(error=>{this.errorMessage("提交失败！")});
              }
          } else {
              return false
          }
      })
      
    },
    shageOpen(type){
      this.bankDialogVisible=true;
      if(type=='add'){
        this.shadeForm.fullName=localStorage.getItem("userNameOrEmailAddress");
        this.shadeForm.payMethod='bankcard';
        this.shadeTitle='添加银行卡';
        this.dataFlag=true;
      }else{
        this.shadeTitle='修改银行卡';
        this.dataFlag=false;
        this.shadeForm=this.typeList[1];
      }
    },
    getData(){
      this.loading=true;
      axios
        .all([
          axios.post(this.$api.incomesetList),
        ])
        .then(
          axios.spread((data) => {
            this.typeList=data.result.item.items;
            if(this.typeList.length>0&&this.typeList.length==1){
              this.id=this.typeList[0].id;
            }else if(this.typeList.length>1){
              for(var i=0;i<this.typeList.length;i++){
                if(this.typeList[i].isDefault){
                  this.id=this.typeList[i].id;
                }
              }
            }
            this.loading=false;
          })
        )
        .catch(err => {
          this.errorMessage('获取数据失败');
          this.loading = false;
        });
    }
  },
  created(){
    this.getData();
  }
};
</script>

<style scoped>
.bankupdate{color:#2DA3FB;margin-left: 8px;font-size: 14px;cursor: pointer;}
</style>