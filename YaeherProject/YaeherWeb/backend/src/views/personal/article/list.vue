<template>
  <section v-loading="loading" class="yh-body">
        <div class="yh-list-header flex-between">
            <el-button type="primary" 
             icon="el-icon-plus"
              @click="routerLink('/article/add','add',0)"
              >添加</el-button>
            <!-- 右操作 -->
             <div>
                <label class="yh-label">文章状态:</label>
                <el-select placeholder="文章状态" v-model="params.checkState" @change="query">
                     <el-option label="全部" :value="''"></el-option>
                     <el-option label="未发布" :value="'created'"></el-option>
                     <el-option label="已发布" :value="'success'"></el-option>
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
                <el-table-column prop="paperTiltle" label="文章标题" width="160" align="center"></el-table-column>
                <el-table-column prop="paperContent" label="文章内容" align="center" :show-overflow-tooltip="true">
                  <div slot-scope="scope">
                    {{scope.row.paperContent.replace(/<[^>]+>|&/g,"").replace(/nbsp;/g,"")}}
                  </div>
                </el-table-column>
                <el-table-column prop="paperAddress" label="文章附件地址" align="center"> 
                </el-table-column>
                <el-table-column prop="paperFrom" label="文章来源" align="center">
                  <div slot-scope="scope">
                    {{scope.row.paperFrom=='doctor'?'医生':'公司'}} 
                  </div>
                </el-table-column>
                <el-table-column prop="doctorName" label="医生姓名" align="center">
                  <div slot-scope="scope">
                    {{scope.row.doctorName==null?'无':scope.row.doctorName}} 
                  </div>
                </el-table-column>
                <el-table-column prop="consultNumber" label="案例编号" align="center"> 
                  <div slot-scope="scope">
                    {{scope.row.consultNumber==null?'无':scope.row.consultNumber}} 
                  </div> 
                </el-table-column>
                <el-table-column prop="checkState" label="文章状态" align="center">
                  <div slot-scope="scope">
                     {{scope.row.checkState=='created'?'未发布':'已发布'}} 
                   </div>
                </el-table-column>
                <el-table-column prop="checkRemark" label="文章备注" align="center">
                  <div slot-scope="scope">
                    {{scope.row.checkRemark==null?'无':scope.row.checkRemark}} 
                  </div> 
                </el-table-column>
                <el-table-column label="审核时间" align="center">
                   <div slot-scope="scope">
                     {{formatTime(scope.row.checkTime)}} 
                   </div>
                </el-table-column>
                <el-table-column prop="checker" label="审核人" align="center"> 
                </el-table-column>
                <!-- <el-table-column prop="readTotal" label="阅读次数" align="center"> 
                </el-table-column>
                <el-table-column prop="upvoteTotal" label="点赞次数" align="center">
                </el-table-column>
                <el-table-column prop="transTotal" label="转发次数" align="center"> 
                </el-table-column>
                <el-table-column prop="collectTotal" label="收藏次数" align="center"> 
                </el-table-column> -->
                <el-table-column label="新增时间"  align="center">
                  <div slot-scope="scope">
                    {{formatTime(scope.row.createdOn)}} 
                  </div>
                </el-table-column>
                <el-table-column label="操作" align="center">
                  <div slot-scope="scope">
                      <yh-button v-if="scope.row.checkState!='success'" type="send" :title="'发布文章'" @click.native="send(scope.row.id)"
                      ></yh-button>
                      <yh-button type="edit" @click.native="routerLink('/article/update','update',scope.row.id)"
                      ></yh-button>
                      <yh-button type="delete" @click.native="deleteBatch($api.articleDelete,scope.row.id)"
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
        checkState:'',
        keyWord:'',
        startTime: '',
        endTime: '',
        skipCount:1,
        maxResultCount:10
      }
    }
  },
  methods:{
    send(id){
      this.$confirm('是否确定发送文章？', '警告', { type: "error" })
      .then(mes => {
          this.$http.post(this.$api.articleSend,{id:id}).then(res=>{
            if(res.result.code==200){
              this.successMessage("文章发布成功");
              this.initTableData(this.$api.articleList,this.params);
            }
          }).catch(error=>{this.errorMessage("文章发布失败");});
      })
      .catch(error => { });
    },
  },
  created(){
    
  },
  activated () {
    this.getRangeTime();
    if(this.params.skipCount==1){
      $(".el-pager .number").eq(0).click();
    }
    this.initTableData(this.$api.articleList,this.params);
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
