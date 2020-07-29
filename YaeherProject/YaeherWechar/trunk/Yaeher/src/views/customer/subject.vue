<template>
    <div class="subject padding-top">
        <mt-header fixed title="科室管理">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
            <mt-button  slot="right">
                <router-link to="/add-subject" class="right-white">添加</router-link>
            </mt-button> 
        </mt-header>
        <div class="content">
            <ul class="group2-list">
                <li v-for="(item, index) in clinicList" :key="index">
                    <div class="flex doctorInfo">
                        <div class="flexCase">
                            <p class="doctor-name">
                                {{item.clinicName}} ({{item.clinicType | clinicSelect}})
                            </p>
                            <div class="operation-control subject-case">
                                <mt-button @click="hintBtn(true, '您确定修改该科室？', item.id, item.orderSort)" type="default">修改</mt-button>
                                <mt-button @click="hintBtn(false, '您确定删除该科室？', item.id)" type="default">删除</mt-button>
                            </div>
                        </div>
                    </div>
                </li>
            </ul>        
        </div>
    </div>
</template>

<script>
import { MessageBox, Toast } from 'mint-ui';
export default {
    data () {
        return {
            clinicList: {}
        }
    },
    filters: {
        clinicSelect (value) {
            const clinicType = ['无', '成人', '儿童']
            return clinicType[value]
        }
    },
    methods: {
        hintBtn(flag, mesg, id1, orderSort1) {
            let _this = this
            if (flag) {
                MessageBox.confirm(mesg).then(action => {
                    this.$router.push({
                        path: '/add-subject',
                        query: {
                            id: id1,
                            orderSort: orderSort1
                        }
                    })
                },function(){
                    console.log('取消了');
                })
            } else {
                MessageBox.confirm(mesg).then(action => {
                    this.instance.deleteClinicC({
                        id: id1
                        })
                            .then((response) => {
                                if(response.data.result.code === 200) {
                                    Toast('删除成功')
                                    this.getClinicList()
                                }
                            })
                            .catch((error) => {

                            }) 
                },function(){
                    console.log('取消了');
                })
            }
        },
        getClinicList () {
            this.instance.clinicPageC({
            })
                .then((response) => {
                    this.clinicList = response.data.result.item
                })
                .catch((error) => {

                }) 
        },
        
    },
    mounted () {
        this.getClinicList()
    }
}
</script>

<style lang="scss">

@import "~assets/sass/base.scss";
.subject {
    .group2-list { 
        font-size: $font-m;
        color: $color-bfont;
        background: $color-wfont;
        li {
            padding: 15px 0; 
            display: flex;
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
                    .subject-case {
                        top: -5px;
                    }
                }
            } 
        }
    }
}


</style>