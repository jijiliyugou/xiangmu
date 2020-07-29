<template>
  <section v-loading="loading" class="yh-body">
        <div class="yh-list-header flex-between">
             <div>
                <label class="yh-label">审核状态:</label>
                <el-select placeholder="审核状态" v-model="params.checkRes" @change="query">
                     <el-option label="全部" :value="''"></el-option>
                     <el-option label="审核中" :value="'checking'"></el-option>
                     <el-option label="审核成功" :value="'success'"></el-option>
                     <el-option label="审核失败" :value="'fail'"></el-option>
                </el-select>
                <label class="yh-label">考试状态:</label>
                <el-select placeholder="考试状态" v-model="params.baseTestRes" @change="query">
                     <el-option label="全部" :value="''"></el-option>
                     <el-option label="未考试" :value="'unExam'"></el-option>
                     <el-option label="通过" :value="'success'"></el-option>
                     <el-option label="不通过" :value="'fail'"></el-option>
                </el-select>
                <label class="yh-label">认证状态:</label>
                <el-select placeholder="认证状态" v-model="params.authCheckRes" @change="query">
                     <el-option label="全部" :value="''"></el-option>
                     <el-option label="已上传" :value="'upload'"></el-option>
                     <el-option label="未上传" :value="'unupload'"></el-option>
                     <el-option label="已认证" :value="'success'"></el-option>
                     <el-option label="认证失败" :value="'fail'"></el-option>
                </el-select>
                <yh-input label="关键字" v-model="params.keyWord" :placeholder="'请输入关键字'" @keyup.enter.native="search"></yh-input>
                <el-date-picker
                    v-model="dateArr"
                    value-format="yyyy-MM-dd"
                    :editable='false'
                    @change="rangeTime"
                    type="daterange"
                    range-separator="至"
                    start-placeholder="开始日期"
                    end-placeholder="结束日期">
                </el-date-picker>
                <el-button @click="query">查询</el-button>               
            </div>
        </div>
        <!-- 表格 -->
        <div class="yh-container">
           <el-table :data="tableData" border style="width: 100%">
                <el-table-column type="index" label="序号" width="50" align="center"></el-table-column>
                <el-table-column label="头像" width="80" align="center">
                  <div slot-scope="scope">
                     <img :src="scope.row.userImage==null?'./../static/images/userlogo.png':scope.row.userImage.replace('cos.ap-guangzhou','picgz')+imgRez" class="userimages">
                  </div>
                </el-table-column>
                <el-table-column prop="doctorName" label="医生姓名" align="center"></el-table-column>
                <!-- <el-table-column prop="address" label="医生住址" align="center">
                  <div slot-scope="scope">
                    <label>{{scope.row.address!=null?scope.row.address:'无'}}</label>
                  </div> 
                </el-table-column> -->
                <el-table-column prop="title" label="职称" align="center"> 
                </el-table-column>
                <el-table-column prop="graduateSchool" label="毕业学校" align="center"> 
                </el-table-column>
                <el-table-column prop="isBelieveTCM" label="是否相信中医" align="center"> 
                  <div slot-scope="scope">
                    <label v-if="scope.row.isBelieveTCM==true">是</label><label v-else>否</label>
                  </div>
                </el-table-column>
                <el-table-column prop="isServiceConscious" label="是否具有服务意识" align="center">
                  <div slot-scope="scope">
                    <label v-if="scope.row.isServiceConscious==true">是</label><label v-else>否</label>
                  </div>
                </el-table-column>
                <el-table-column prop="phoneNumber" label="手机号码" align="center"> 
                </el-table-column>
                <el-table-column prop="wechatNum" label="关联微信号" align="center"> 
                </el-table-column>
                <el-table-column prop="recommenderName" label="推荐人" align="center">
                  <div slot-scope="scope">
                    <label>{{scope.row.recommenderName!=''?scope.row.recommenderName:'无'}}</label>
                  </div> 
                </el-table-column>
                <el-table-column prop="checkRes" label="审核结果" align="center"> 
                  <div slot-scope="scope">
                    <label v-if="scope.row.checkRes=='fail'">审核失败</label>
                    <label v-else-if="scope.row.checkRes=='success'">审核成功</label>
                    <label v-else-if="scope.row.checkRes=='checking'">审核中</label>
                  </div>
                </el-table-column>
                <el-table-column label="考试状态" align="center"> 
                  <div slot-scope="scope">
                    <label v-if="scope.row.baseTestRes=='fail' || scope.row.simTestRes=='fail'">不通过</label>
                    <label v-else-if="scope.row.baseTestRes=='success' && scope.row.simTestRes=='success'">通过</label>
                    <label v-else>未考试</label>
                  </div>
                </el-table-column>
                <el-table-column prop="authCheckRes" label="认证状态" align="center">
                   <div slot-scope="scope">
                    <label v-if="scope.row.authCheckRes=='fail'">认证失败</label>
                    <label v-else-if="scope.row.authCheckRes=='success'">已认证</label>
                    <label v-else-if="scope.row.authCheckRes=='unupload'">未上传</label>
                    <label v-else-if="scope.row.authCheckRes=='upload'">已上传</label>
                   </div>
                </el-table-column>
                <el-table-column label="新增时间"  align="center">
                  <div slot-scope="scope">
                    {{formatTime(scope.row.createdOn)}} 
                  </div>
                </el-table-column>
                <el-table-column label="操作" align="center">
                  <div slot-scope="scope">
                      <yh-button v-if="roleType('Manager')&&scope.row.checkRes!='success'" type="check" @click.native="routerLink('/doctor/view','Check',scope.row.id)"
                      ></yh-button>
                      <yh-button v-if="roleType('CustomerService')&&scope.row.checkRes=='success'&&scope.row.baseTestRes!='success'&&scope.row.simTestRes!='success'" type="check" :title="'考试'" @click.native="routerLink('/doctor/view','Test',scope.row.id)"
                      ></yh-button>
                      <yh-button v-if="roleType('CustomerService')&&scope.row.authCheckRes!='success'&&scope.row.authCheckRes!='unupload'&&scope.row.baseTestRes=='success' && scope.row.simTestRes=='success'" type="check" :title="'认证'" @click.native="routerLink('/doctor/view','Authen',scope.row.id)"
                      ></yh-button>
                      <yh-button type="view" @click.native="routerLink('/doctor/view','view',scope.row.id)"
                      ></yh-button>
                      <yh-button :title="'科室设置'" type="set" @click.native="routerLink('/doctor/partset','update',scope.row.id)"
                      ></yh-button>
                      <yh-button type="set" @click.native="routerLink('/doctor/labelset','update',scope.row.id)"
                      ></yh-button>
                  </div>
                 </el-table-column>
           </el-table>
        </div>
        <!-- 表格底部 -->
        <div class="yh-list-footer">
                <div class="yh-left">                 
                  <!-- <el-button :disabled="disabled" @click="deleteBatch($api.topicDelete,ids)"                 
                  >批量删除</el-button> -->
                </div>
                <!-- 分页组件 -->
              <yh-pagination :total="total" @change="getPages"></yh-pagination>
        </div> 
  </section>
</template>

<script>
import listMixins from '@/mixins/list'
export default {
  name:'keepin',
  mixins:[listMixins],
  data() {
    return {
      imgRez:localStorage.getItem('imRez')==undefined?'':localStorage.getItem('imRez'),
      dateArr:'',
      params: {//只需要业务参数
        checkRes:'',
        baseTestRes:'',
        authCheckRes:'',
        keyWord:'',
        startTime: '',
        endTime: '',
        skipCount:1,
        maxResultCount:10
      }
    }
  },
  methods:{
  },
  created(){
    
  },
  activated () {
    if(this.params.skipCount==1){
      $(".el-pager .number").eq(0).click();
    }
    this.initTableData(this.$api.doctorList,this.params);
  },
  beforeRouteLeave (to, from, next) {
    let topath=to.path.split('/').pop();
    if(topath=='add'||topath=='list'){
      this.params.skipCount=1;
    }
    next();
  }
};
</script>
