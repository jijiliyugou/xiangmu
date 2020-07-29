<template>
  <section v-loading="loading" class="yh-body">
        <div class="yh-list-header flex-between">
             <div>
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
                     <img :src="scope.row.userImage==null?'./../static/images/userlogo.png':scope.row.userImage+imgRez" class="userimages">
                  </div>
                </el-table-column>
                <el-table-column prop="doctorName" label="医生姓名" align="center"></el-table-column>
                <el-table-column prop="title" label="职称" align="center"> 
                </el-table-column>
                <el-table-column prop="hospitalName" label="所在医院" align="center">
                </el-table-column>
                <el-table-column prop="department" label="科室" align="center"> 
                </el-table-column>
                <el-table-column prop="graduateSchool" label="毕业学校" align="center">
                </el-table-column>
                <el-table-column prop="workYear" label="工作年限" align="center"> 
                </el-table-column>
                <el-table-column prop="phoneNumber" label="手机号码" align="center"> 
                </el-table-column>
                <el-table-column prop="wechatNum" label="关联微信号" align="center"> 
                </el-table-column>
                <el-table-column prop="orderTotal" label="订单数量" align="center"> 
                </el-table-column>
                <el-table-column prop="refundTotal" label="退单数量" align="center"> 
                </el-table-column>
                <el-table-column prop="doctorLevel" label="评分" align="center"> 
                </el-table-column>
                <el-table-column prop="checkState" label="审核状态" align="center"> 
                  <div slot-scope="scope">
                    <label v-if="scope.row.checkState=='fail'">不通过</label>
                    <label v-else-if="scope.row.checkState=='success'">通过</label>
                    <label v-else-if="scope.row.checkState=='checking'">审核中</label>
                  </div>
                </el-table-column>
                <el-table-column label="新增时间"  align="center">
                  <div slot-scope="scope">
                    {{formatTime(scope.row.createdOn)}} 
                  </div>
                </el-table-column>
                <el-table-column label="操作" align="center">
                  <div slot-scope="scope">
                      <yh-button type="check" v-if="scope.row.checkState=='checking'" @click.native="routerLink('/qualitycheck/view','Check',scope.row.qualityCommitteeRegisterId)"
                      ></yh-button>
                      <yh-button type="view" @click.native="routerLink('/qualitycheck/view','view',scope.row.qualityCommitteeRegisterId)"
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
    this.getRangeTime();
    if(this.params.skipCount==1){
      $(".el-pager .number").eq(0).click();
    }
    this.initTableData(this.$api.qualitycheckList,this.params);
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
