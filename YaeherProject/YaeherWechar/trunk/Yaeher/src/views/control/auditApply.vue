<template>
    <div class="audit-apply padding-top">
        <mt-header fixed title="申请质控委员详情">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="audit-apply-case">
                <div class="flex doctorInfo">
                    <router-link :to="{path: '/doctor-detail-patient', query: {id: applyDetail.id}}" class="img-case">
                        <img :src="applyDetail.userImage" />
                    </router-link>
                    <div class="flexCase">
                        <p class="doctor-name">
                            {{applyDetail.doctorName}}<span v-if="'qualitystart' === applyDetail.applyState">（申请）</span>
                            <span v-if="'qualitystart' != applyDetail.applyState">（取消）</span>
                            <span v-if="applyDetail.checkState === 'fail'" class="hint red-hint">不通过</span>
                            <span v-if="applyDetail.checkState === 'checking'" class="hint red-hint">待处理</span>
                            <span v-if="applyDetail.checkState === 'success'" class="hint green-hint">通过</span>
                        </p>
                        <p class="star-font"><star :star=applyDetail.averageEvaluate></star> {{applyDetail.averageEvaluate}}</p>
                        <p>{{applyDetail.department}}</p>
                        <p><span>{{applyDetail.hospitalName}}</span> <span>{{applyDetail.title}}</span></p>
                        <div class="operation-control">
                        </div>
                    </div>
                </div>
                <div class="audit-case">
                    <p class="apply-title">申请原因：</p>
                    <p>{{applyDetail.qualityApplyRemark}}</p>
                    <div v-if="applyDetail.checkState === 'checking'" class="audit-btn-case">
                        <mt-button @click="hintBtn(false, '您确定不通过质控委员？')" type="default">不通过</mt-button>
                        <mt-button @click="hintBtn(true, '您确定通过质控委员？')" type="primary">通过</mt-button>
                    </div>
                </div>
                
            </div>
        </div>
    </div>
</template>

<script>
import { MessageBox, Toast } from 'mint-ui';
import Star from 'components/star/star'
export default {
    components: {
      Star
    },
    data () {
        return {
            id: 0,
            applyDetail: {},
            qualityReason: '',
        }
    },    
    methods: {
        hintBtn(flag, mesg) {
            let _this = this
            if (flag) {
                MessageBox.confirm(mesg).then(action => {
                    this.applyControl('success', '', '通过成功')
                },function(){
                    console.log('取消了');
                })
            } else {
                MessageBox.prompt(mesg).then(({ value, action }) => {
                    if (!value) {
                        Toast('评价原因不能为空')
                        return
                    } else {
                        this.applyControl('fail', value, '不通过成功')
                    }
                },function(){
                    console.log('取消了');
                })
                
            }
        },
        getApplyDetail () { // 获取申请质控委员详情
            const id = this.id
            this.instance.qualityCommitteeRegisterByIdC({
                id
            })
                .then((response) => {
                    this.applyDetail = response.data.result.item
                })
                .catch((error) => {
                }) 
        },
        applyControl (qualityState1, checkRemark1, mess) { // 质控操作
            const qualityCommitteeRegisterID = this.id
            const qualityState = qualityState1
            const checkRemark = checkRemark1
            this.instance.qualityCommittee({
                qualityCommitteeRegisterID,
                qualityState,
                checkRemark
            })
                .then((response) => {
                    if(response.data.result.code === 200) { 
                        Toast(mess)
                        this.$router.go(-1)
                    }
                })
                .catch((error) => {
                })
        }
        
    },
    mounted () {
        this.getApplyDetail()
    },
    created () {
        this.id = parseInt(this.$route.query.id)
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.audit-apply {
    .audit-apply-case {
        background: $color-wfont;
        padding: 10px;
        .doctorInfo {
            width: 100%;
            .img-case {
                width: 60px;
                height: 60px;
                display: block;
                img{
                    display: block;
                    width: 60px;
                    height: 60px;
                    border-radius: 5px;
                }
            }
            .flexCase {
                padding: 0 10px; 
                position: relative;
                flex: 1;
                p {
                    font-size: $font-m;
                    color: $color-bfont;
                    padding-bottom: 2px;
                }
                .doctor-name {
                    font-size: $font-xl;
                    color: $color-afont;
                    .hint {
                        float: right;
                        font-size: $font-l;
                    }
                }
                .star-font {
                    color: $color-star;
                }
                .operation-control {
                    position: absolute;
                    right: 0;
                    top: 20px;
                    .mint-button {
                        height: 30px;
                        font-size: $font-l;
                        margin-left: 10px;
                    }
                }
            }
        } 
        .audit-case {
            padding: 10px 0;
            p {
                color: $color-bfont;
                font-size: $font-l;
            }
            .apply-title {
                padding-top: 5px;
            }
            .audit-btn-case {
                padding-top: 10px;
                text-align: right;
                .mint-button {
                    height: 30px;
                    font-size: $font-l;
                    margin-left: 10px;
                }
            }
        }
        
    }
}


</style>