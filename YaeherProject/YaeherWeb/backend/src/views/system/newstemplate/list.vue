<template>
  <section v-loading="loading" class="yh-body">
        <div class="yh-list-header flex-between">
            <el-button type="primary"
              @click="getData"
              >获取</el-button>
        </div>
        <!-- 表格 -->
        <div class="yh-container">
           <el-table :data="tableData" border style="width: 100%">
              <el-table-column type="index" label="序号" width="50" align="center"></el-table-column>
              <el-table-column prop="templateCode" label="模板类型" align="center"> 
              </el-table-column>
              <el-table-column prop="title" label="模板类型名称" align="center"> 
              </el-table-column>
              <el-table-column prop="wecharTitle" label="微信模板名称" width="160" align="center"></el-table-column>
              <el-table-column prop="templateId" label="微信模板编号" align="center"> 
              </el-table-column>
              <el-table-column prop="content" label="模板内容" align="center"> 
              </el-table-column>
              <el-table-column prop="example" label="模板示例" align="center"> 
              </el-table-column>
              <el-table-column label="新增时间"  align="center">
                <div slot-scope="scope">
                  {{formatTime(scope.row.createdOn)}} 
                </div>
              </el-table-column>
              <el-table-column label="操作" align="center">
                <div slot-scope="scope">
                  <yh-button v-if="scope.row.templateCode!=''" type="plus" title="添加发送消息模板" @click.native="routerLink('/newstemplate/sendadd','list',scope.row.templateCode)"
                      ></yh-button>
                  <yh-button v-if="scope.row.templateCode!=''" type="view" title="查看发送消息模板" @click.native="routerLink('/newstemplate/sendlist','list',scope.row.templateCode)"
                    ></yh-button>
                    <yh-button v-if="scope.row.templateCode==''" type="edit" @click.native="routerLink('/newstemplate/update','update',scope.row.id)"
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
    getData(){
      this.loading=true;
      this.$http.post(this.$api.WXNewsModelGet).then(res=>{
        if(res.result.code==200){
          this.loading=false;
        }
      });
    },
  },
  created(){
    
  },
  activated () {
    this.getRangeTime();
    if(this.params.skipCount==1){
      $(".el-pager .number").eq(0).click();
    }
    this.initTableData(this.$api.WXNewsModelList,this.params);
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
