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
                <el-table-column prop="doctorName" label="医生姓名" width="160" align="center"></el-table-column>
                <el-table-column prop="address" label="医生住址" align="center">
                </el-table-column>
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
                </el-table-column>
                <el-table-column prop="checkRes" label="审核结果" align="center"> 
                  <div slot-scope="scope">
                    <label v-if="scope.row.checkRes=='fail'">审核失败</label>
                    <label v-else-if="scope.row.checkRes=='success'">审核成功</label>
                    <label v-else-if="scope.row.checkRes=='checking'">审核中</label>
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
                      <yh-button type="check" @click.native="routerLink('/doctor/view','Check',scope.row.id)"
                      ></yh-button>
                      <yh-button type="check" :title="'认证'" @click.native="routerLink('/doctor/view','Authen',scope.row.id)"
                      ></yh-button>
                      <yh-button type="check" :title="'考试'" @click.native="routerLink('/doctor/view','Test',scope.row.id)"
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
  mixins:[listMixins],
  data() {
    return {
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
    this.getRangeTime();
    this.initTableData(this.$api.doctorList,this.params);
  }
};
</script>
