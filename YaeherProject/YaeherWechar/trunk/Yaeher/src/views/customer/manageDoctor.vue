<template>
    <div class="manage-doctor padding-top">
        <mt-header fixed title="医生管理">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content listAbout">
            <div class="searchCase">
                <div class="searchBox">
                    <input @keypress="searchKeypress" ref="inputText" type="search" v-model="keyWord" placeholder="请输入医生的名字" />
                    <div @click="searchDoctor(true)">搜索</div>
                </div>
            </div>
            <ul v-infinite-scroll="searchDoctor"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10" class="group2-list">
                <li v-for="(item, index) in doctorList" :key="index">
                    <div class="flex doctorInfo">
                        <router-link :to="{path: '/doctor-detail-patient', query:{id: item.id}}" class="img-case">
                            <img v-if="item.userImage != null" :src="item.userImage" :onerror="stallDefaultImg" align="avater" />
                            <img v-if="item.userImage === null" src="../../assets/image/logo.jpg" alt="" />
                        </router-link>
                        <div class="flexCase">
                            <p class="doctor-name">
                                {{item.doctorName}}
                            </p>
                            <p class="star-font"><star :star="item.AverageEvaluate"></star> {{item.AverageEvaluate}}</p>
                            <p><span>{{item.hospitalName}}</span> <span>{{item.title}}</span></p>
                            <div class="operation-control">
                                <mt-button @click="goClinic(item.id)" type="default">科室</mt-button>
                                <mt-button v-if="item.onlineState != 'online'" @click="hintBtn(true, '您确定上线该医生？', item.id, 'online',item.doctorOnlineRecordId )" type="default">上线</mt-button>
                                <mt-button v-if="item.onlineState != 'offline'" @click="hintBtn(true, '您确定下线该医生？', item.id, 'offline', item.doctorOnlineRecordId )" type="default">下线</mt-button>
                                <mt-button @click="hintBtn(false, '您确定删除该医生？',item.id, item.doctorOnlineRecordId )" type="default">删除</mt-button>
                            </div>
                        </div>
                    </div>
                </li>

            </ul>        
        </div>
    </div>
</template>

<script>
import Star from 'components/star/star'
import { MessageBox, Toast } from 'mint-ui';
export default {
    components: {
      Star
    },
    data () {
        return {
            keyWord: '',
            doctorList: [],
            skipCount: 1,
			maxResultCount: 10,
			totalPage: 2,
            stallDefaultImg: 'this.src="' + require('assets/image/logo.jpg') + '"',
            loading: true
        }
    },
    methods: {
        hintBtn(flag, mesg, id1, key, id2) {
            if (flag) { // 上下线医生
                MessageBox.confirm(mesg).then(action => {
                    this.instance.updateDoctorOnlineRecordC({
                        id: id2,
                        onlineState: key
                    })
                        .then((response) => {
                            if (key === 'online') Toast('上线成功')
                            if (key != 'online') Toast('下线成功')
                            setTimeout(function(){
                                window.location.reload()
                            }, 1000)
                            
                            
                        })
                        .catch((error) => {
                        }) 
                    
                },function(){
                    console.log('取消了');
                })
            } else {
                MessageBox.confirm(mesg).then(action => {
                    this.instance.deleteYaeherDoctorC({
                            id: id1
                        })
                            .then((response) => {
                                if (response.data.result.code === 200) {
                                    Toast('删除成功')
                                    setTimeout(function(){
                                        window.location.reload()
                                    }, 500)
                                }
                            })
                            .catch((error) => {
                            }) 
                },function(){
                    console.log('取消了');
                })
            }
        },
        goClinic (id1) {
            this.$router.push({ 
                path: '/select-clinic',
                query: {
                    id: id1
                }
            })
        },
        searchDoctor(flag) { // 搜索医生
            this.loading = true
            if (flag) {
                this.skipCount = 1
                this.totalPage = 2
            }
            const keyWord = this.keyWord
            const maxResultCount  = this.maxResultCount 
			const skipCount = this.skipCount
			if (skipCount > this.totalPage) {
				if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.$refs.inputText.blur()
            this.instance.doctorSearch({
                keyWord,
                skipCount,
                maxResultCount,
                onlineState: ''
            })
                .then((response) => {
                    setTimeout(() => {
                        this.loading = false
                        let moreFlag = response.data.result.item.items
                        if (!moreFlag || moreFlag.length===0) {
							this.loading = true
                        }
                        if (flag) this.doctorList = []
                        this.doctorList = this.doctorList.concat(response.data.result.item.items)
                        this.skipCount ++
                        this.totalPage = response.data.result.item.totalPage
                        console.log(this.skipCount, this.totalPage)
                    }, 100);    
                })
                .catch((error) => {
                    this.loading = true
                    console.log('医生列表获取失败')
                }) 
            
        },
        searchKeypress (event) {
            if (event.keyCode == 13) {
                this.searchDoctor(true)
            } 
        },
        
    },
    mounted () {
        this.searchDoctor()
    }
}
</script>

<style lang="scss">

@import "~assets/sass/base.scss";
.manage-doctor {
    .listAbout {
        position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;
    }
    .group2-list { 
        font-size: $font-m;
        color: $color-bfont;
        background: $color-wfont;
        padding: 0 10px;
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
                        top: 10px;
                        .mint-button {
                            height: 30px;
                            font-size: $font-l;
                            margin-left: 5px;
                        }
                    }
                }
            } 
        }
    }
}


</style>