<template>
    <div class="addReplay padding-top">
        <mt-header fixed title="快捷回复">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
            <!-- <mt-button v-if="id ===0"   slot="right">
                <a @click="createReplay" class="right-white">保存</a>
            </mt-button>  -->
        </mt-header>
        <div class="content">
            <div class="introduce-case">
                <mt-field  @input = "descInput" label="简介" placeholder="填写快捷回复信息" type="textarea" rows="8" v-model="content" :attr="{ maxlength: maxReplyLength }"></mt-field>
                <p>可输入{{remark}}字</p>
                
                <div v-if="id ===0" class="okCase">
                    <mt-button @click="createReplay" type="primary" class="mint-button--large">添加</mt-button>
                </div>
                <div v-if="id !=0" class="okCase">
                    <mt-button @click="updateReplay" type="primary" class="mint-button--large">保存</mt-button>
                </div>

                <div v-if="id !=0"  class="okCase">
                    <mt-button @click="deleteReplay" type="default" class="mint-button--large">删除</mt-button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
export default {
    data () {
        return {
            id: 0,
            content: '',
            maxReplyLength: 5000,
            remark: 5000
        }
	},
	methods: {
        descInput() {
            let txtVal = this.content.length
            this.remark = this.maxReplyLength - txtVal
        },
        getUserInfo() { // 请求快捷回复详情
            const id = this.id
            this.instance.quickReplyByIdD({
                id
            })
                .then((response) => {
                    if(response.data.result.code===200) {
                        this.content = response.data.result.item.content
                    }
                })
                .catch((error) => {
                }) 
        },
        createReplay () { // 添加快捷回复
            const content = this.content
            if(!content) {
                Toast('快捷回复不能为空')
                return
            }
            this.instance.createQuickReplyD({
                content
            })
                .then((response) => {
                    if(response.data.result.code===200) {
                        Toast('添加成功')
                        this.$router.go(-1)
                    }
                })
                .catch((error) => {
                }) 
        },
        updateReplay() { // 修改快捷回复
            const content = this.content
            const id = this.id
            if(!content) {
                Toast('快捷回复不能为空')
                return
            }
            this.instance.updateQuickReplyD({
                id,
                content
            })
                .then((response) => {
                    Toast('修改成功')
                    this.$router.go(-1)
                })    
                .catch((error) => {
                }) 
        },
        deleteReplay() { // 删除快捷回复
            const id = this.id
            this.instance.deleteQuickReplyD({
                id
            })
                .then((response) => {
                    Toast('删除成功')
                    this.$router.go(-1)
                })    
                .catch((error) => {
                }) 
        }
    },
    mounted () {
        
    },
    created () {
        let id1 = this.$route.query.id
        if (id1) {
            this.id = parseInt(id1)
            this.getUserInfo()
        }
    }
}    
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.addReplay {
	position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: #fff;
    z-index: 3000;
    .introduce-case {
        padding: 10px 10px 20px; 
        background: $color-wfont;
        .mint-cell-wrapper {
            background-image: none;
        }
        .mint-cell-title {
            display: none;
        }
        .okCase {
            padding-top: 20px;
        }
        p {
            text-align: right;
            color: $color-bfont;
            font-size: $font-m;
            padding-right: 5px;
        }
    }
}

</style>