<template>
  <section v-loading="loading" class="yh-body">
        <div class="yh-list-header flex-between">
            <el-button type="primary" 
             icon="el-icon-plus"
              @click="routerLink('/answer/add','add',0)"
              >添加</el-button>
            <!-- 右操作 -->
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
                <el-table-column prop="descriptionTiltle" label="问题描述" width="160" align="center" :show-overflow-tooltip="true">
                </el-table-column>
                <el-table-column prop="answer" label="问题回答" align="center" :show-overflow-tooltip="true">
                  <div slot-scope="scope">
                    {{scope.row.answer.replace(/<[^>]+>/g,"")}} 
                  </div>
                </el-table-column>
                <el-table-column prop="checker" label="审核人" align="center"> 
                </el-table-column>
                <el-table-column prop="checkState" label="审核状态" align="center"> 
                </el-table-column>
                <el-table-column prop="checkRemark" label="审核备注" align="center"> 
                </el-table-column>
                <el-table-column prop="checkTime" label="审核时间" align="center">
                  <div slot-scope="scope">
                    {{formatTime(scope.row.checkTime)}} 
                  </div> 
                </el-table-column>
                <el-table-column prop="readTotal" label="阅读量" align="center"> 
                </el-table-column>
                <el-table-column prop="upvoteTotal" label="点赞次数" align="center">
                </el-table-column>
                <el-table-column prop="transTotal" label="转发次数" align="center"> 
                </el-table-column>
                <el-table-column prop="collectTotal" label="收藏次数" align="center"> 
                </el-table-column>
                <el-table-column label="新增时间"  align="center">
                  <div slot-scope="scope">
                    {{formatTime(scope.row.createdOn)}} 
                  </div>
                </el-table-column>
                <el-table-column label="操作" align="center">
                  <div slot-scope="scope">
                      <yh-button type="edit" @click.native="routerLink('/answer/update','update',scope.row.id)"
                      ></yh-button>
                       <yh-button type="delete" @click.native="deleteBatch($api.answerDelete,scope.row.id)"
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
    this.initTableData(this.$api.answerList,this.params);
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
