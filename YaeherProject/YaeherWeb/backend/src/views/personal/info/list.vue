<template>
  <section class="yh-body" v-loading="loading">
    <div class="peicebox">
      <div class="b-bnav">
          <div class="rect-nav"></div><span class="rn-a">基本信息</span>
      </div>
      <el-form  ref="form" :model="dataInfo"
          class="yh-form" label-width="150px">
        <el-form-item label="头像：" class="flex-100">
            <div slot-scope="scope" class="imagebox">
               <img :src="dataInfo.userImage" class="userimages">
            </div>
        </el-form-item>
        <el-form-item label="姓名：" class="flex-50">
            <span class="itemval" v-text="dataInfo.fullName"></span>
        </el-form-item>
        <el-form-item label="系统登录名：" class="flex-50">
            <span class="itemval" v-text="dataInfo.loginName"></span>
        </el-form-item>
        <el-form-item label="电话：" class="flex-50">
            <span class="itemval" v-text="dataInfo.phoneNumber"></span><label class="mobile" @click="mobileDialogVisible=true">修改</label>
        </el-form-item>
        <el-form-item label="邮箱：" class="flex-50">
            <span class="itemval" v-text="dataInfo.email"></span>
        </el-form-item>
        <el-form-item label="性别：" class="flex-50">
            <div slot-scope="scope">
               <label v-if="dataInfo.sex==1">男</label><label v-else>女</label>
            </div>
        </el-form-item>
        <el-form-item label="生日：" class="flex-50">
            <div slot-scope="scope">
              {{formatDate(dataInfo.birthday)}}
            </div> 
        </el-form-item>
        <el-form-item v-if="qualityLabel&&roleType('Doctor')" :label="applyText" class="flex-50">
            <span class="itemval" v-text="qualityResult"></span>
        </el-form-item>
        <el-form-item label="认证资料：" class="flex-50" v-if="roleType('Doctor')">
          <span class="itemval" v-if="authData=='fail'">审核失败</span>
          <span class="itemval" v-else-if="authData=='success'">审核成功</span>
          <span class="itemval" v-else-if="authData=='upload'">审核中</span>
          <span class="itemval" v-else-if="authData=='unupload'">未上传</span>
        </el-form-item>
        <el-form-item label="标签：" class="flex-100" v-if="roleType('Doctor')">
            <el-tag 
             size="mini"
             v-for="tag in dynamicTags"
             closable
             :key="tag.lableID"
             :disable-transitions="true"
             @close="tabClose(tag.lableID)">{{tag.lableName}}</el-tag>
        </el-form-item>
        <el-form-item label="简介：" class="flex-100" v-if="roleType('Doctor')">
            <span class="itemval" v-text="resume==''?'无':resume"></span>
        </el-form-item>
      </el-form>       
      <div class="form-footer">
          <!-- <el-button type="success" @click.native="routerLink('/info/update','update',userId)"
             >修改</el-button> -->
          <el-button type="primary" v-if="applyBtn&&roleType('Doctor')" @click.native="qualityshage(true)"
             >申请质控委员</el-button>
          <el-button type="primary" v-if="cancelBtn&&roleType('Doctor')" @click.native="qualityshage(false)"
             >取消质控委员</el-button>
          <el-button type="primary" v-if="authData=='unupload'&&roleType('Doctor')" @click.native="routerLink('/info/identity','add',0)"
           >认证资料</el-button>
           <el-button type="primary" v-if="authData=='fail'&&roleType('Doctor')" @click.native="routerLink('/info/identityupdate','update',0)"
           >修改认证资料</el-button>
           <el-button v-if="roleType('Doctor')" type="primary" @click.native="introduceDialogVisible=true"
             >简介编辑</el-button>
           <el-button v-if="roleType('Doctor')" type="primary" @click.native="labelGet()"
             >添加标签</el-button>
           <el-button type="primary" @click.native="pwdDialogVisible=true"
             >修改密码</el-button>
      </div>
    </div>
    <div class="peicebox" v-if="roleType('Doctor')">
      <div class="b-bnav">
          <div class="rect-nav"></div><span class="rn-a">从业经历</span>
      </div>
      <div class="form-footer">
          <el-button type="primary" @click.native="shagepage()"
             >添加</el-button>
      </div>
      <div class="tablebox">
        <el-table :data="tableData" border style="width: 100%">
            <el-table-column type="index" label="序号" width="50" align="center"></el-table-column>
            <el-table-column prop="hospitalName" label="医院" width="160" align="center"></el-table-column>
            <el-table-column prop="department" label="科室" align="center"> 
            </el-table-column>
            <el-table-column prop="workYear" label="工作年限" align="center"> 
            </el-table-column>
            <el-table-column label="操作" align="center">
              <div slot-scope="scope">
                  <yh-button type="edit" @click.native="shagepage({id:scope.row.id,hospitalName:scope.row.hospitalName,department:scope.row.department,workYear:scope.row.workYear})"
                  ></yh-button>
                  <yh-button type="delete" @click.native="deleteItem($api.workDelete,scope.row.id)"
                  ></yh-button>
              </div>
             </el-table-column>
       </el-table>      
    </div>
    </div> 
    <div class="peicebox" v-if="roleType('Doctor')">
      <div class="b-bnav">
          <div class="rect-nav"></div><span class="rn-a">添加科室</span>
      </div>
      <div class="form-footer">
        <el-button type="primary" @click.native="routerLink('/info/clinicadd','add',0)"
           >添加</el-button>
      </div>
      <div class="tablebox">
        <el-table :data="clinicData" border style="width: 100%">
            <el-table-column type="index" label="序号" width="50" align="center"></el-table-column>
            <el-table-column prop="clinicName" label="科室" align="center"> 
            </el-table-column>
            <el-table-column prop="checkRes" label="状态" align="center"></el-table-column>
            <el-table-column label="操作" align="center">
              <div slot-scope="scope">
                  <yh-button v-if="scope.row.checkResCode!='checking'&&scope.row.checkRes!=''" type="edit" @click.native="routerLink('/info/clinicupdate','update',scope.row.id)"
                  ></yh-button>
                  <yh-button v-if="scope.row.checkRes==''" type="edit" @click.native="routerLink('/info/clinicupdate',scope.row.clinicID+','+scope.row.clinicName,0)"
                  ></yh-button>
              </div>
             </el-table-column>
        </el-table>      
      </div>
    </div> 
    <!-- 从业经历 -->
    <el-dialog
      class="shadebox"
      :title="shadeTitle"
      :visible.sync="centerDialogVisible"
      width="30%"
      height="400px"
      center>
      <el-form ref="form" :model="shadeForm" :rules="rules" label-width="100px">
        <el-form-item label="医院：" prop="hospitalName">
          <el-input v-model="shadeForm.hospitalName"></el-input>
        </el-form-item>
        <el-form-item label="科室：" prop="department">
          <el-input v-model="shadeForm.department"></el-input>
        </el-form-item>
        <el-form-item label="工作年限：" prop="workYear">
          <el-input v-model="shadeForm.workYear"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="centerDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="putIn()">确 定</el-button>
      </span>
    </el-dialog> 
    <!-- 申请质控 -->
    <el-dialog
      class="shadebox"
      :title="qualityTitle"
      :visible.sync="qualityDialogVisible"
      width="30%"
      height="400px"
      center>
      <el-form ref="form" :model="qualitydata" label-width="100px">
          <div class="qualitytext">
            <p class="qt">平台的发展离不开质量控制，更好的质控能让怡禾上的每一位医生有更好的口碑和收入，质控委员对医生是一种荣誉，同时怡禾也会为质控工作支付一些酬劳。
            质控委员将承担一下工作：</p>
            <p>1.复核低星咨询的咨询质量（平均每月数个）</p>
            <p>2.审核新上线医生的咨询质量</p>
            <p>3.审核相关专科拟分享咨询案例的质量</p>
            <p>4.审核相关专科的科普文章、课程的知识点</p>
            <p class="qt">熟悉怡禾操作规范的要求，并符合以下条件的医生，可申请为医生质控委员：</p>
            <p>1.在平台提供咨询服务超过半年</p>
            <p>2.咨询总量超过300单</p>
            <p>3.评分不低于4.9分</p>
          </div>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="qualityDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="qualityApply()">确 定</el-button>
      </span>
    </el-dialog>
    <!-- 个人简介 -->
    <el-dialog
      class="shadebox"
      :title="'个人简介'"
      :visible.sync="introduceDialogVisible"
      width="30%"
      height="400px"
      center>
      <el-form ref="form" label-width="100px">
        <el-form-item label="个人简介：">
          <el-input placeholder="请填写个人简介" type="textarea" :rows="6" v-model="resume" resize="none"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="introduceDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="introduceApply()">确 定</el-button>
      </span>
    </el-dialog>  
    <!-- 修改手机号码 -->
    <el-dialog
      class="shadebox"
      :title="'修改手机号码'"
      :visible.sync="mobileDialogVisible"
      width="380px"
      height="400px"
      center>
      <el-form ref="form" label-width="100px">
        <el-form-item label="手机号码：" prop="mobileNum">
          <el-input v-model="mobileNum"></el-input>
        </el-form-item>
        <el-form-item label="验证码：">
          <el-input v-model="code" style="width:120px;"></el-input><el-button type="primary" @click="codeApply()" style="float:right;width:100px;" :disabled="codeStatus">{{mobliebtn}}</el-button>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button type="primary" @click="mobileApply()">确定</el-button>
      </span>
    </el-dialog>  
    <!-- 添加标签 -->
    <el-dialog
      class="shadebox"
      :title="'添加标签'"
      :visible.sync="labelDialogVisible"
      width="500px"
      center>
      <el-form ref="form" label-width="100px" class="tablist">
        <el-form-item label="标签：">
          <!-- <el-tag 
             size="mini"
             v-for="tag in tabList"
             :key="tag.id"
             @click.native="labelAdd(tag.lableName)"
             >{{tag.lableName}}</el-tag> -->
          <el-input v-model="tagname" placeholder="请输入标签名"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="labelDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="labelAdd()">确 定</el-button>
      </span>
    </el-dialog>
    <!-- 修改密码 -->
    <el-dialog
      class="shadebox"
      :title="'修改密码'"
      :visible.sync="pwdDialogVisible"
      width="380px"
      height="400px"
      center>
      <el-form ref="form" label-width="100px">
        <el-form-item label="手机号码：">
          <label v-text="mobileNum"></label>
        </el-form-item>
        <el-form-item label="验证码：">
          <el-input v-model="passcode" style="width:120px;"></el-input><el-button type="primary" @click="passcodeApply()" style="float:right;width:100px;" :disabled="passcodeStatus">{{passmobliebtn}}</el-button>
        </el-form-item>
        <el-form-item label="新密码：">
          <el-input v-model="pass" type="password"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button type="primary" @click="pwdApply()">确定</el-button>
      </span>
    </el-dialog>     
</section>
</template>
<script>
import va from "@/rules";
import md5 from 'js-md5';
export default {
  data() {
    let required = va.required();
    let number = va.number();
    return {
      applyText:'申请质控委员：',
      loading:false,
      qualityLabel:false,
      authData:'unupload',
      shadeTitle:'',
      qualityTitle:'',
      dataInfo:{},
      tagname:'',
      dataFlag:false,
      applyBtn:true,
      cancelBtn:false,
      qualitydata:{//申请取消质控提交参数
        applyState:'',
        applyRemark:''
      },
      qualityResult:'',
      qualityType:'',
      centerDialogVisible: false,//从业经历弹窗
      qualityDialogVisible:false,//申请质控弹窗
      introduceDialogVisible:false,//个人简历添加
      labelDialogVisible:false,//标签添加
      pwdDialogVisible:false,//修改密码
      number:90,
      mobileNum:'',
      pass:'',
      code:'',
      passcode:'',
      passcodeStatus:false,
      passmobliebtn:'发送验证码',
      codeStatus:false,
      mobliebtn:'发送验证码',
      mobileDialogVisible:false,//手机号码
      tableData:[],
      clinicData:[],
      resume:'',
      resumeid:'',
      shadeForm:{
      },
      dataInfo:{
      },
      dynamicTags:[],
      tabList:[],
      rules: {//校验规则
        hospitalName: [required],
        department: [required],
        workYear: [required,number],
        applyRemark:[required]
      }
    }
  },
  methods:{
    labelAdd(){
      let reg = /^[A-Za-z0-9\u4e00-\u9fa5]{1,20}$/;
      if (!reg.test(this.tagname)) {
          this.errorMessage('请输入1-20个字符长度的标签');
          return false;
      }
      this.$http.post(this.$api.tagAdd,{lableName:this.tagname}).then(res=>{
        if(res.result.code==200){
           this.getTabData();
           this.labelDialogVisible=false;
           this.successMessage('添加成功');
        }
      }).catch(error=>{this.errorMessage('添加失败');});
    },
    labelGet(){
      /*this.$http.post(this.$api.tagList,{doctorID:localStorage.getItem('doctorId')}).then(res=>{
        if(res.result.code==200){
          this.tabList=res.result.item;*/
          this.labelDialogVisible=true;
          this.tagname="";
       /* }
      }).catch(error=>{
        this.errorMessage("获取用户标签失败");
      });*/
    },
    tabClose(id) {
      this.$http.post(this.$api.tagDelete,{id:id}).then(res=>{
        if(res.result.code==200){
           this.getTabData();
          this.successMessage('删除成功');
        }
      }).catch(error=>{this.errorMessage('删除失败');});
    },
    getTabData(){
      this.$http.post(this.$api.tagData,{maxResultCount: 100}).then(res=>{
        if(res.result.code==200){
          this.dynamicTags=res.result.item.items;
        }
      }).catch(error=>{
        this.errorMessage("获取用户标签失败");
      });
    },
    shagepage(type){//弹窗
      this.centerDialogVisible=true;
      if(type){
        this.shadeForm=type;
        this.dataFlag=false;
        this.shadeTitle='从业经历修改';
      }else{
        this.shadeForm={};
        this.dataFlag=true;
        this.shadeTitle='从业经历添加';
      }
    },
    putIn(){//添加修改共用方法
      let form = this.$refs['form'];
      form.validate((valid) => {//验证方法
          if (valid) {
              if(this.dataFlag){
                this.$http.post(this.$api.workAdd,this.shadeForm).then(res=>{
                  if(res.result.code=200){
                    this.successMessage("添加成功");
                    this.centerDialogVisible=false;
                    this.getData();
                  }
                }).catch(error=>{this.errorMessage("提交失败！")});
              }else{
                this.$http.post(this.$api.workUpdate,this.shadeForm).then(res=>{
                  if(res.result.code=200){
                    this.successMessage("修改成功");
                    this.centerDialogVisible=false;
                    this.getData();
                  }
                }).catch(error=>{this.errorMessage("提交失败！")});
              }
          } else {
              return false
          }
      })
      
    },
    getData(){//获取数据
      this.loading=true;
      this.$http.post(this.$api.infoallData).then(res=>{
        if(res.result.code==200){
          this.loading=false;
          this.dataInfo=res.result.item.yaeherUser;
          this.authData=res.result.item.yaeherDoctor.authCheckRes;
          this.mobileNum=res.result.item.yaeherDoctor.phoneNumber;
          this.dataInfo.phoneNumber=this.mobileNum;
          this.tableData=res.result.item.doctorEmployments;
          this.resume=res.result.item.yaeherDoctor.resume;
          this.resumeid=res.result.item.yaeherDoctor.id;
        }
      }).catch(error=>{
        this.loading=false;
        this.errorMessage("获取数据失败！")
      });
    },
    deleteItem(url,id) { //删除
        this.$confirm('是否确定删除？', '警告', { type: "error" })
            .then(mes => {
                this.$http.post(url, { id: id }).then(res => {
                    if (res.result.code == "200") {
                        this.successMessage('删除成功');
                        this.getData();
                    }
                });
            })
            .catch(error => { });
    },
    qualityshage(type){//弹窗
      this.qualityDialogVisible=true;
      this.qualitydata.applyRemark="";
      if(type){
        this.qualitydata.applyState=this.qualityType[0].code;
        this.qualityTitle='申请质控委员';
      }else{
        this.qualitydata.applyState=this.qualityType[1].code;
        this.qualityTitle='取消质控委员';
      }
    },
    qualityGetData(){
      this.$http.post(this.$api.qualityStatus
      ).then(res=>{
        if(res.result.code==200){
          if(res.result.item!=null){
            if(res.result.item.length>0){
              this.qualityLabel=true;
              const type=res.result.item[0].applyState;
              const state=res.result.item[0].checkState;
              if(state=="checking"){
                this.applyBtn=false;
                this.cancelBtn=false;
              }else if((state=="success"&&type=="qualitystart")||(state=="fail"&&type=="qualitystop")){
                this.applyBtn=false;
                this.cancelBtn=true;
              }else if((state=="success"&&type=="qualitystop")||(state=="fail"&&type=="qualitystart")){
                this.applyBtn=true;
                this.cancelBtn=false;
              }
              if(type=="qualitystart"){
                this.applyText="申请质控委员：";
              }else if(type=="qualitystop"){
                this.applyText="取消质控委员：";
              }
              if(res.result.item[0].checkState=="checking"){
                this.qualityResult="审核中";
              }else if(res.result.item[0].checkState=="fail"){
                this.qualityResult="审核不通过";
              }else if(res.result.item[0].checkState=="success"){
                this.qualityResult="审核通过";
              }
            }else{
              this.qualityLabel=false;
            }
          }
        }
      }); 
    },
    qualityApply(){//申请质控
      /*if(this.qualitydata.applyRemark.trim()==""){
        this.errorMessage("原因描述不能为空");
        return false;
      }*/
      this.$http.post(this.$api.qualityApply,this.qualitydata).then(res=>{
        if(res.result.code==200){
          this.successMessage("提交申请成功");
          this.qualityGetData();
          this.qualityDialogVisible=false;
        }
      }).catch(error=>{
        this.errorMessage("提交失败");
        this.qualityDialogVisible=false;
      });
    },
    introduceApply(){//个人简介提交
      this.$http.post(this.$api.resumeApply,{resume:this.resume,id:this.resumeid}).then(res=>{
        if(res.result.code==200){
          this.successMessage("提交成功");
          this.introduceDialogVisible=false;
        }
      }).catch(error=>{
        this.errorMessage("提交失败");
        this.introduceDialogVisible=false;
      });
    },
    codeApply(){//发送验证码
      let reg = /^1\d{10}$/;
      if (!reg.test(this.mobileNum)) {
          this.errorMessage('请输入正确的手机号');
          return false;
      }
      function settime() {//倒计时
        if (countDown == 0) {
            $this.codeStatus=false;
            $this.mobliebtn="再次获取";
            countDown = num;
            return;
        } else {
            $this.codeStatus=true;
            $this.mobliebtn=countDown + "s";
            countDown--;
            setTimeout(function () {
                settime();
            }, 1000)
        }
      }
      const $this=this;
      var num = 90;
      var countDown = num;
      this.$http.post(this.$api.codeSend,{messageType: "Authentication",phoneNumber:this.mobileNum}).then(res=>{
        if(res.result.code==200){
          settime();
        }
      }).catch(error=>{this.errorMessage("发送验证失败")});
    },
    passcodeApply(){//发送验证码  
      function settime() {//倒计时
        if (countDown == 0) {
            $this.passcodeStatus=false;
            $this.passmobliebtn="再次获取";
            countDown = num;
            return;
        } else {
            $this.passcodeStatus=true;
            $this.passmobliebtn=countDown + "s";
            countDown--;
            setTimeout(function () {
                settime();
            }, 1000)
        }
      }
      const $this=this;
      var num = 90;
      var countDown = num;
      this.$http.post(this.$api.codeSend,{messageType: "ChangePassword",phoneNumber:this.mobileNum}).then(res=>{
        if(res.result.code==200){
          settime();
        }
      }).catch(error=>{this.errorMessage("发送验证失败")});
    },
    mobileApply(){//修改手机号
      this.$http.post(this.$api.phoneUpdate,{messageType: "Authentication",phoneNumber:this.mobileNum,verificationCode:this.code,id:localStorage.getItem('userId')}).then(res=>{
        if(res.result.code==200){
          this.successMessage("修改成功");
          this.mobileDialogVisible=false;
          this.dataInfo.phoneNumber=this.mobileNum;
        }
      }).catch(error=>{this.errorMessage("修改失败")});
    },
    pwdApply(){//修改密码
      if(this.pass==""){
        this.errorMessage("密码不能为空");
      }
      this.$http.post(this.$api.passUpdate,{phoneNumber:this.mobileNum,verificationCode:this.passcode,loginPwd:md5(this.pass)}).then(res=>{
        if(res.result.code==200){
          this.successMessage("修改成功");
          this.pwdDialogVisible=false;
        }
      }).catch(error=>{this.errorMessage("修改失败")});
    }
  },
  created(){
    this.getData();
    this.getTabData();
    this.$http.post(this.$api.doctorclinicList).then(res=>{
      if(res.result.code==200){
        this.clinicData=res.result.item;
      }
    }).catch(error=>{
      this.errorMessage("获取科室数据失败");
    });
    
    this.$http.post(this.$api.qualityParam,{type:'ConfigPar',systemCode: 'QualityState'}).then(res=>{
      if(res.result.code==200){
        this.qualityType=res.result.item;
      }
    });
    this.qualityGetData();
  }
};
</script>

<style scoped>
.yh-form .el-form-item{padding:0px;}
.tablebox{padding:0px 60px 30px 60px;}
.imagebox{width: 30px;overflow: hidden;}
.form-footer{padding:15px 60px;}
.imagebox img{display: block;width: 100%;}
.itemval{display: inline-block;max-width: 500px;line-height: 20px;margin-top: 6px;}
.peicebox{background: #fff;border-bottom: 10px solid #f6f6f6;}
.peicebox:last-child{border-bottom: 0px;}
.b-bnav{width: 100%;height: 40px;border-bottom: 1px solid #ececec;font-size: 12px;}
.rect-nav{background: #42485a;width: 2px;height: 24px;margin-top: 8px;float: left;margin-left: 6px;}
.rn-a{float: left;line-height: 40px;margin-left: 12px;color:#333;}
.mobile{color:#2DA3FB;margin-left: 8px;font-size: 14px;cursor: pointer;}
.el-tag{margin-right: 8px;}
.tablist .el-tag{cursor: pointer;}
.qualitytext p{font-size: 14px;color:#333;}
.qualitytext .qt{font-size: 14px;font-weight: 700;color:#333;}
</style>