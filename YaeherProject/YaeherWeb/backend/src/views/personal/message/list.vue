<template>
  <section class="yh-body" v-loading="loading">
    <div class="messagebox">
      <div class="m-left">
        <p class="userlist">用户列表</p><p class="unread" v-if="unreadNum!=0" @click="reading()">{{'新消息'+unreadNum+'条'}}</p>
        <div class="newbox" id="newBox">
          <div v-for="(item,index) in sessionstate" :class="item.nickName==nickName?'item active':'item'" @click="getItem(item.id,item.nickName,item.consultantOpenID)">
            <div class="userimage">
              <img :src="item.userImage">
            </div>
            <div class="roledetail">
              <p class="rolecall">{{item.nickName}}</p>
              <p class="time">{{formatTime(item.acceptTime)}}</p>
            </div>
            <div class="status">{{item.isReady==true?'已读':'未读'}}</div>
            <label :class="item.acceptState=='2'?'top el-icon-star-off':'top el-icon-star-on'" @click.stop="topUp(item.id,item.acceptState)" :title="item.acceptState=='2'?'置顶':'取消置顶'"></label>
          </div>
        </div>
      </div>
      <div class="m-right">
        <div class="m-rt">
          <p class="rolename">{{nickName}}</p>
        </div>
        <div class="m-rm" ref="contentbox">
          <div v-for="(item,index) in sessionitem" :class="item.messageFrom=='Consultant'?'m-customer':'m-self'">
              <div v-if="item.msgType=='text'">
                <div class="con">{{item.content}}</div>
                <p :class="item.messageFrom=='Consultant'?'datetimel':'datetimer'">{{formatTime(item.createdOn)}}</p>
              </div>
              <div v-if="item.msgType=='image'">
                <div class="msgimg"><img :src="item.picUrl"></div>
                <p :class="item.messageFrom=='Consultant'?'datetimel':'datetimer'">{{formatTime(item.createdOn)}}</p>
              </div>
          </div>
        </div>
        <div class="m-toolbox" v-if="id!=''">
          <el-upload
            action="https://jsonplaceholder.typicode.com/posts/"
            class="avatar-uploader"
            :show-file-list="false"
            :before-upload="beforeupload"
            accept="image/*"
            >
            <i class="el-icon-picture-outline" title="图片"></i>
          </el-upload>
        </div>
        <div class="m-rb" v-if="id!=''">
          <textarea class="mytextbox" v-model="message" placeholder="请编辑发送的内容"></textarea>
          <input type="button" :disabled="able" value="发送" class="sendbtn" @click="dataSend()">
        </div>
      </div>
    </div>
    <!-- 分页组件 -->
    <yh-pagination :total="total" @change="getPages"></yh-pagination>  
  </section>
</template>
<script>
export default {
  data() {
    return {
      params: {//只需要业务参数
        keyWord:'',
        startTime: '',
        endTime: '',
        skipCount:1,
        maxResultCount:10
      },
      imgData:{
        serviceType:'',
        mediaType:''
      },
      startTime:'',
      unreadNum:0,
      loading:false,
      sessionstate:[],
      sessionitem:[],
      nickName:'',
      message:'',
      total: 0, //列表总数
      openId:'',
      id:'',
      timer:null,
      able:false,
      uploadData:{},
      imageUrl:'',
      scrollflag:false//滚动条是否滚到底部
    }
  },
  methods:{
    beforeupload(file){//上传之前
      //console.log(file);
    },
    uploadImage(key,file){//上传图片
      $this.$http.post($this.$api.messageSend,{fromUserName:$this.openId,msgType:"image",picUrl:$this.imageUrl}).then(res=>{
        if(res.result.code==200){
          $this.imageUrl="";
          $this.able=false;
          $this.getItemData($this.id,$this.nickName,$this.openId);
        }
      }).catch(error=>{
        $this.loading=false;
        $this.errorMessage("发送图片失败！")
      });
    },
    reading(){//未读消息点击
      this.unreadNum=0;
      if(this.params.skipCount==1){
        this.getData(this.params);
      }
      $(".el-pager .number").eq(0).click();
    },
    topUp(id,status){
      var $status;
      if(status=="2"){
        $status="1";
      }else if(status=="1"){
        $status="2";
      }
      this.$http.post(this.$api.messageTop,{id:id,acceptState:$status}).then(res=>{
        if(res.result.code==200){
          this.loading=false;
          clearTimeout(this.timer);
          this.getData(this.params);
          if($status=="1"){
            this.successMessage("置顶成功");
          }else{
            this.successMessage("已取消置顶");
          }
          
        }
      }).catch(error=>{
        this.loading=false;
        this.errorMessage("操作失败！")
      });
    },
    getData(params){//获取用户列表数据
      this.$http.post(this.$api.messageList,params).then(res=>{
        if(res.result.code){
          this.loading=false;
          this.sessionstate=res.result.item.items;
          this.total = res.result.item.totalCount;
          this.startTime=this.secondTime(new Date());
        }
      }).catch(error=>{
        this.loading=false;
        this.errorMessage("获取数据失败！")
      });
    },
    getUnread(){//获取未读消息
      this.$http.post(this.$api.messageunRead,{startTime:this.startTime}).then(res=>{
        if(res.result.code=200){
          this.unreadNum=res.result.item;
          let $this=this;
          this.timer=setTimeout(function(){
            $this.getUnread();
            if($this.id!=''){
              $this.getItemData($this.id,$this.nickName,$this.openId);
            }
          },10000)
        }
      }).catch(error=>{
        this.errorMessage("获取未读消息失败！")
      });
    },
    getItem(id,name,openid){//用户列表点击
      this.scrollflag=true;
      this.getItemData(id,name,openid);
    },
    getItemData(id,name,openid){//单个用户数据
      this.$http.post(this.$api.messageData,{id:id}).then(res=>{
        if(res.result.code==200){
          this.sessionitem=res.result.item;
          this.nickName=name;
          this.openId=openid;
          this.id=id;
          if(this.scrollflag){
            this.$nextTick(function () {
              this.$refs.contentbox.scrollTop = this.$refs.contentbox.scrollHeight;
              this.scrollflag=false;
            })
          } 
        }
      }).catch(error=>{
        this.loading=false;
        this.errorMessage("获取用户数据失败！")
      });
    },
    dataSend(){//消息发送
      if(this.message.trim()==""){
        this.errorMessage("发送内容不能为空");
        return false;
      }
      this.able=true;
      this.$http.post(this.$api.messageSend,{fromUserName:this.openId,msgType:"text",content:this.message}).then(res=>{
        if(res.result.code==200){
          this.message="";
          this.able=false;
          this.scrollflag=true;
          this.getItemData(this.id,this.nickName,this.openId);
        }
      }).catch(error=>{
        this.loading=false;
        this.errorMessage("发送消息失败！")
      });
    },
    getPages(skipCount, maxResultCount) {
      //获取翻页数据
      this.params.skipCount = skipCount;
      this.params.maxResultCount = maxResultCount;
      this.getData(this.params);
    }
  },
  destroyed(){
    clearTimeout(this.timer);
    this.timer=null;
  },
  created(){
    this.loading=false;
    this.startTime=this.secondTime(new Date());
    if(this.$route.query.id!=undefined){
      this.$http.post(this.$api.messageJoinin,{consultantID:this.$route.query.id}).then(res=>{
        if(res.result.code==200){
          this.nickName=res.result.item.nickName;
          this.id=res.result.item.id;
          this.openId=res.result.item.consultantOpenID;
          this.getItemData(this.id,this.nickName,this.openId);
          this.getData(this.params);
          this.getUnread();
        }
      }).catch(error=>{this.errorMessage('获取用户数据失败');});
    }else{
      this.getData(this.params);
      this.getUnread();
    } 
  }
};
</script>

<style scoped>
.messagebox{width: 802px;height: 620px;border:1px solid #e5e5e5;margin-left: 32px;margin-top: 32px;}
.m-left{width: 220px;height: 100%;border-right: 1px solid #e5e5e5;float: left;position: relative;}
.newbox{height: 586px;overflow-y:auto;}
.userlist{font-size: 14px;line-height: 32px;text-indent: 12px;color:#B6B4B6;border-bottom: 1px solid #e5e5e5;}
.m-right{width: 580px;height: 620px;float: left;}
.m-rt{width: 100%;height: 58px;border-bottom: 1px solid #e5e5e5;overflow: hidden;position: relative;}
.rolename{font-size: 18px;font-weight: 800;text-indent: 20px;margin-top: 20px;}
.m-rm{width: 100%;height: 350px;border-bottom: 1px solid #e5e5e5;overflow-y:auto;padding-bottom: 8px;}
.m-toolbox{width: 100%;height: 30px;font-size: 22px;line-height: 30px;}
.el-icon-picture-outline{margin-left: 8px;/* cursor: pointer; */}
.m-rb{width: 100%;height: 177px;position: relative;}
.mytextbox{height: 176px;width: 100%;resize: none;padding:8px 6px 8px 10px;font-size: 14px;border:none;}
.item{width: 100%;height: 60px;border-bottom:1px solid #e5e5e5;position: relative;cursor: pointer;}
.item:hover{background: #f5f5f5;}
.status{font-size: 12px;line-height: 20px;position: absolute;right: 6px;bottom:4px;color:#8C8D8f;}
.userimage{float: left;width: 46px;height: 46px;margin: 7px;}
.userimage img{display: block;width: 100%;height: 100%;}
.m-customer .msgimg{width: 100px;height: 100px;padding: 4px;background: #CCE4FC;border:1px solid #C6DEFF;float: left;border-radius: 4px;margin-left: 10px;}
.m-self .msgimg{width: 100px;height: 100px;padding: 4px;background: #CCE4FC;border:1px solid #C6DEFF;float: right;border-radius: 4px;margin-right: 10px;}
.msgimg img{display: block;width: 100%;height: 100%;}
.roledetail{float: left;width: 120px;}
.rolecall{font-size: 14px;width: 110px;white-space: nowrap;overflow: hidden;text-overflow: ellipsis;line-height: 38px;}
.time{font-size: 12px;color:#8C8D8F;}
.m-customer{float: left;width: 60%;font-size: 14px;margin-top: 8px;}
.m-self{float: right;width: 60%;font-size: 14px;margin-top: 8px;}
.m-customer .con{display: block;padding: 4px;background: #CCE4FC;border:1px solid #C6DEFF;float: left;border-radius: 4px;margin-left: 10px;word-break: break-all;}
.m-self .con{display: block;padding: 4px;background: #CCE4FC;border:1px solid #C6DEFF;float: right;border-radius: 4px;margin-right: 10px;word-break: break-all;}
.sendbtn{width: 42px;height: 26px;background: #f94f50;color:#fff;font-size: 14px;border:none;position: absolute;right: 4px;bottom: 2px;border-radius: 4px;cursor: pointer;}
.datetimel{width: 100%;float: left;font-size: 12px;color:#8C8D8f;margin-left: 12px;padding-top: 4px;}
.datetimer{width: 100%;overflow: hidden;font-size: 12px;color:#8C8D8f;margin-right: 12px;padding-top: 4px;float: right;text-align: right;}
.active{background: #f5f5f5;}
.top{font-size: 18px;position: absolute;color:#E6A23C;right: 4px;top: 4px;}
.top:hover{cursor: pointer;}
.unread{float:left;position: absolute;left: 0px;top: -10px;font-size: 12px;width: 100%;background: #CCE4FC;text-align: center;cursor: pointer;}
.yh-pagination{margin-left: 38px;margin-top: 12px;overflow: hidden;}
</style>