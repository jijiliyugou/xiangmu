<template>
       <a href="javascript:void(0)" 
       class="yh-table-button iconfont" 
       :class="type"
       :title="title" 
       > 
       </a>
    
</template>

<script>
export default {
    name:'yh-button',
    props: {
        type:{
            type:String,
            default:'edit'
        }
    },
    data(){
        return{}   
    },
    computed:{
        title(){
            let titie="";
                if(this.type=='edit'){
                     titie='修改'; 
                }
                if(this.type=='delete'){
                     titie='删除'; 
                }
                if(this.type=='rename'){
                     titie='重命名'; 
                }
                if(this.type=='view'){
                     titie='查看'; 
                }
                if(this.type=='power'){
                    titie='权限管理'
                }
                if(this.type=='down'){
                    titie='下载'
                }
                if(this.type=='user'){
                    titie='分配角色'
                }
                if(this.type=='reply'){
                    titie='处理'
                }
                if(this.type=='retreat'){
                    titie='退单'
                }
                if(this.type=='check'){
                    titie='审核'
                }
                if(this.type=='set'){
                    titie='标签设置'
                }
                if(this.type=='issue'){
                    titie='发布文章'
                }
                if(this.type=='plus'){
                    titie='添加'
                }
                if(this.type=='send'){
                    titie='推送'
                }
                if(this.type=='newSend'){
                    titie='发送消息'
                }
            return titie;
        }
    },
    methods:{

    }
}
</script>

<style scoped lang="scss">
    .yh-table-button{
        display: inline-block;
        width: 24px;
        height: 24px;
        line-height:1;
        border-radius: 4px;
        background: #000;
        color: #fff;
         position: relative;
         text-align: center;
         font-size: 14px;
         top:-2px;
         margin-left: 2px;
         transition: 0.2s;
         &:first-child{
               margin-left: 0px;
         }
         &:hover{
             opacity: 0.65;
         }
     &.edit{
         background: #17d57e;
        &::after{
            content: '\e662';
            position: relative;
           top:5px;
        }        
     }
      &.delete{
         background: #ff8b53;
        &::after{
            content: "\e604";
            position: relative;
            top:4px;
            font-size:12px; 
        }        
     }  
      &.rename{
         background: #d573ea;
        &::after{
            content: "\e670";
            position: relative;
            top:4px;
            font-size:12px; 
        }        
     
     }
     &.view{
         background: #50c3e4;
        &::after{
            content: "\e797";
            position: relative;
            top:4px;
            font-size:12px; 
        }        
     }
     &.user{
        background: #ffae38;
         &::after{
            content: "\e659";
            position: relative;
            top:4px;
            font-size:14px; 
        }    
     }
     &.down{
        background: #409EFF;
         &::after{
            content: "\eb8e";
            position: relative;
            top:4px;
            font-size:14px; 
        }  
     }
     &.power{
        background: #409EFF;
         &::after{
            content: "\eb8d";
            position: relative;
            top:4px;
            font-size:14px; 
        }  
     }
     &.reply{
        background: #409EFF;
         &::after{
            content: "\e6ea";
            position: relative;
            top:4px;
            font-size:14px; 
        }  
     }
     &.retreat{
        background: #409EFF;
         &::after{
            content: "\e7af";
            position: relative;
            top:4px;
            font-size:14px; 
        }  
     }
     &.check{
        background: #409EFF;
         &::after{
            content: "\e72c";
            position: relative;
            top:4px;
            font-size:14px; 
        }  
     }
     &.set{
        background: #409EFF;
         &::after{
            content: "\e71c";
            position: relative;
            top:4px;
            font-size:14px; 
        }  
     }
     &.issue{
        background: #409EFF;
         &::after{
            content: "\e6ea";
            position: relative;
            top:4px;
            font-size:14px; 
        }  
     }
     &.plus{
        background: #409EFF;
         &::after{
            content: "\e634";
            position: relative;
            top:4px;
            font-size:14px; 
        }  
     }
     &.send{
        background: #409EFF;
         &::after{
            content: "\e60d";
            position: relative;
            top:4px;
            font-size:14px; 
        }  
     }
     &.newSend{
        background: #409EFF;
         &::after{
            content: "\e605";
            position: relative;
            top:4px;
            font-size:14px; 
        }  
     }
     }
</style>
