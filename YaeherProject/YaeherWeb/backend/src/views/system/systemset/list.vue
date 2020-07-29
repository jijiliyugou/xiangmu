<template>
  <section v-loading="loading" class="yh-body">
        <div class="yh-list-header flex-between">
             <div>
                <yh-input label="关键字" v-model="params.keyWord" :placeholder="'请输入关键字'" @keyup.enter.native="search"></yh-input>
                <!-- <el-date-picker
                    v-model="dateArr"
                    value-format="yyyy-MM-dd"
                    :editable='false'
                    @change="rangeTime"
                    type="daterange"
                    range-separator="至"
                    start-placeholder="开始日期"
                    end-placeholder="结束日期">
                </el-date-picker> -->
                <el-button @click="query">查询</el-button>                
            </div>
        </div>
        <!-- 表格 -->
        <div class="yh-container">
           <el-table :data="tableData" border style="width: 100%">
                <el-table-column type="index" label="序号" width="50" align="center"></el-table-column>
                <el-table-column prop="systemName" label="系统名称" width="160" align="center"></el-table-column>
                <el-table-column prop="systemType" label="系统类型" align="center">
                </el-table-column>
                <el-table-column prop="tenPayKey" label="支付秘钥" align="center"> 
                </el-table-column>
                <el-table-column prop="tenPayMchId" label="支付ID" align="center"> 
                </el-table-column>
                <el-table-column prop="tenPayNotify" label="tenPayNotify" align="center"> 
                </el-table-column>
                <el-table-column prop="tenPayRefundNotify" label="tenPayRefundNotify" align="center"> 
                </el-table-column>
                <el-table-column prop="accessKeyID" label="accessKeyID" align="center"> 
                </el-table-column>
                <el-table-column prop="accessSecret" label="accessSecret" align="center"> 
                </el-table-column>
                <el-table-column prop="appID" label="appID" align="center"> 
                </el-table-column>
                <el-table-column prop="appKey" label="appKey" align="center"> 
                </el-table-column>
                <el-table-column prop="appSecret" label="appSecret" align="center"> 
                </el-table-column>
                <el-table-column prop="callCenterNumber" label="callCenterNumber" align="center"> 
                </el-table-column>
                <el-table-column prop="refreshTokenCode" label="refreshTokenCode" align="center"> 
                </el-table-column>
                <el-table-column label="操作" align="center">
                  <div slot-scope="scope">
                      <yh-button type="edit" @click.native="routerLink('/systemset/update','update',scope.row.id)"
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
    this.initTableData(this.$api.systemSetList,this.params);
  }
};
</script>
