<template>
  <section v-loading="loading" class="yh-body">
        <div class="yh-list-header flex-between">
            <el-button type="primary" 
             icon="el-icon-plus"
              @click="routerLink('/part/add','add',0)"
              >添加</el-button>
            <!-- 右操作 -->
             <div>
                <label class="yh-label">科室类型:</label>
                <el-select placeholder="科室类型" v-model="params.clinicType" @change="query">
                     <el-option label="全部" :value="0"></el-option>
                     <el-option label="成人" :value="1"></el-option>
                     <el-option label="儿童" :value="2"></el-option>
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
                <el-table-column prop="clinicName" label="科室名称" width="160" align="center"></el-table-column>
                <el-table-column prop="clinicType" label="科室类型" align="center">
                  <div slot-scope="scope">
                    <label v-if="scope.row.clinicType=='1'">成人</label>
                    <label v-else>儿童</label>
                  </div>
                </el-table-column>
                <el-table-column prop="orderSort" label="排序序号" align="center"> 
                </el-table-column>
                <el-table-column prop="clinicIntro" label="科室介绍" align="center"> 
                </el-table-column>
                <el-table-column label="新增时间"  align="center">
                  <div slot-scope="scope">
                    {{formatTime(scope.row.createdOn)}} 
                  </div>
                </el-table-column>
                <el-table-column label="操作" align="center">
                  <div slot-scope="scope">
                      <!-- <yh-button type="check" @click.native="routerLink('/part/powerset','update',scope.row.id)"
                      ></yh-button> -->
                      <yh-button type="edit" @click.native="routerLink('/part/update','update',scope.row.id)"
                      ></yh-button>
                       <yh-button type="delete" @click.native="deleteBatch($api.partDelete,scope.row.id)"
                       ></yh-button>
                       <yh-button :title="'医生设置'" type="set" @click.native="routerLink('/part/doctorset','update',scope.row.id)"
                      ></yh-button>
                      <yh-button type="set" @click.native="routerLink('/part/labelset','update',scope.row.id)"
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
        clinicType:0,
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
    this.initTableData(this.$api.partList,this.params);
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
